using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalVilla_API.Models;

public class VillaAmenities
{
    public int Id  { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public DateTime CreatedAt  { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    // relationships
    [ForeignKey(nameof(Villa))]
    public int VillaId { get; set; }
    
    public Villa?  Villa { get; set; }
}