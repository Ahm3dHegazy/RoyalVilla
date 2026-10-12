namespace RoyalVilla_API.DTOs;

public class VillaAmenitiesDto
{
    public int Id  { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string? VillaName { get; set; }
}
