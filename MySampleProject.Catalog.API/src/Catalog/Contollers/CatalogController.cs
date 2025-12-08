using MyEshopOnContainers.MySampleProject.Catalog.API.src.Catalog;


//--- GET ROUTES ---//
// 1. Get All Items                         (Done)
// 2. Get Items By Id 
// 3. Get Item By Id                        (DONE)
// 4. Get Item By Name                      (DONE)
// 5. Get Item By Picture
// 6. Get Items By Semantic Relevance
// 7. Get Items By Brand and Type Id
// 8. Get Items By Brand Id
// 9. Get Catalog Types
// 10. Get Catalog Brands

//--- MODIFY ROUTES ---//
// 1. Create Item                           (DONE)
// 2. Update Item                           (DONE)
// 3. Delete Item By Id                     (DONE)

using Microsoft.AspNetCore.Mvc;

namespace MyEshopOnContainers.MySampleProject.Catalog.API.src.Catalog.API.Contollers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService _catalogService;


        public CatalogController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CatalogItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CatalogItem>>> GetAllItemsAsync()
        {
            try
            {
                var items = await _catalogService.GetAllItemsAsync();
                return Ok(items);
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve items.");    
            }
        }

         [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItem>> GetItemAsync(int id)
        {
            try
            {
                var catalogItem = await _catalogService.GetItemAsync(id);

                if (catalogItem == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(catalogItem);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve Catalog Item.");
            }
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<CatalogItem>> GetItemByNameAsync(string name)
        {
            var catalogItems = await _catalogService.GetItemByNameAsync(name);

            if(catalogItems == null || !catalogItems.Any())
            {
                return NotFound(new ProblemDetails{ Detail = $"Item with name '{name}' not found"});
            }
            return Ok(catalogItems);
        }

        [HttpPost]
        public async Task<ActionResult<CatalogItem>> CreateItemAsync([FromBody] CatalogItem catalogItem)
        {
            try
            {
                var newItem = await _catalogService.CreateItemAsync(catalogItem);
                return Ok(newItem);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);       
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] CatalogItem itemToUpdate)
        {
            var updatedItem = await _catalogService.UpdateItemAsync(id, itemToUpdate);

            if(updatedItem == null)
            {
                return NotFound(new ProblemDetails{Detail = $"Item with id {id} not found"});
            }
            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var deletedItem = await _catalogService.DeleteItemAsync(id);

            if(deletedItem == null)
            {
                return NotFound(new ProblemDetails{Detail = $"Item with id {id} not found"});
            }

            return Ok(deletedItem);
        }
    }
}