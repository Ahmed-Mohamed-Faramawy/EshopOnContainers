
using Microsoft.EntityFrameworkCore;

public class CatalogService
{
    private readonly CatalogContext _catalogContext;

    public CatalogService(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<IEnumerable<CatalogItem>> GetAllItemsAsync()
    {
        return await _catalogContext.CatalogItems.ToListAsync();
    }

    public  async Task<CatalogItem> GetItemAsync(int id)
    {
        return await _catalogContext.CatalogItems.FindAsync(id);
    }

    public async Task<IEnumerable<CatalogItem>> GetItemByNameAsync(string name)
    {
        var query = _catalogContext.CatalogItems.Where(item => item.Name.StartsWith(name));

        return await query
                        .OrderBy(item => item.Name)
                        .ToListAsync();
    }

    public async Task<CatalogItem> CreateItemAsync(CatalogItem catalogItem)
    {
        // Ensure required Foreign Keys exist before saving
        var brandExists = await _catalogContext.CatalogBrands.AnyAsync(b => b.Id == catalogItem.CatalogBrandId);
        var typeExists = await _catalogContext.CatalogTypes.AnyAsync(t => t.Id == catalogItem.CatalogTypeId);

        if(!brandExists || !typeExists)
        {
            throw new ArgumentException("Creation Failed, either brand or type does not exist!");
        }

        try
        {
            await _catalogContext.CatalogItems.AddAsync(catalogItem);
            await _catalogContext.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Database error while creating item!");
        }
        return catalogItem;
    }

    public async Task<CatalogItem> UpdateItemAsync(int id, CatalogItem catalogItem)
    {
        // 1. Read the item so that it is now Tracked by DbContext
        var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(i => i.Id == id);

        if(item == null)
        {
            return null;
        }

        // 2. Modify the tracked item
        item.Price = catalogItem.Price;
        item.Description = catalogItem.Description;

        // 3. Save: EF Core detects the changes and issues UPDATE SQL.
        await _catalogContext.SaveChangesAsync();

        return item;
    }

    public async Task<CatalogItem> DeleteItemAsync(int id)
    {
        var item = await _catalogContext.CatalogItems.FindAsync(id);

        if(item == null)
        {
            return null;
        }

        _catalogContext.CatalogItems.Remove(item);
        await _catalogContext.SaveChangesAsync();

        return item;
    }
}