

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

[Table("catalogItems")]
public class CatalogItem
{
    [Key]
    public int Id  {get; set;}

    public string Name {get; set;}

    public string? Description {get; set;}

    public decimal Price {get; set;}

    public string? PictureFileName {get; set;}

    public int CatalogTypeId {get; set;}

    public CatalogType? CatalogType {get; set;}

    public int CatalogBrandId {get; set;}

    public CatalogBrand? CatalogBrand {get; set;}

    // Quantity in Stock
    public int AvailableStock {get; set;}
    
    // Available Stock at which we should reorder
    public int ReorderThreshold {get; set;}

    // Maximum number of units that can be in-stock at any time (due to physicial/logistical constraints in warehouses)
    public int MaxStockThreshold { get; set; }

}