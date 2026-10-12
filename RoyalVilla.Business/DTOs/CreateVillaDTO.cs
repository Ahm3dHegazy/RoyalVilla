namespace RoyalVilla_API.DTOs;

public class CreateVillaDto
{
    [Required]
    [MaxLength(30)]
    public string? Name { get; set; }
    
    [MaxLength(200)]
    public string? Details { get; set; }
    
    public double Rate { get; set; }
    
    public int Sqft { get; set; }
    
    public int Occupancy { get; set; }
    
    public string? ImageUrl { get; set; }
}