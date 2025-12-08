
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("catalogBrand")]
public class CatalogBrand
{
    public CatalogBrand(string brand)
    {
        Brand = brand;
    }
    
    [Key]
    public int Id {get; set;}

    [Required]
    public string Brand {get; set;}
}