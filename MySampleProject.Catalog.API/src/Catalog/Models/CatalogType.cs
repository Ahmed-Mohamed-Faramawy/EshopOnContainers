
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("catalogType")]
public class CatalogType
{
    public CatalogType(string type)
    {
        Type = type;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Type { get; set; }
}