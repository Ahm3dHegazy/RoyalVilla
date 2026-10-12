namespace RoyalVilla_API.DTOs;

public class CreateVillaAmenitiesDto
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(300)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int VillaId { get; set; }
}
