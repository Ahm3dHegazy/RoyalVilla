using System.ComponentModel.DataAnnotations;

namespace RoyalVillaWeb.Models;

public sealed class VillaDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Details { get; set; }
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
}

public sealed class AmenityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? VillaName { get; set; }
}

public sealed class VillasPageViewModel
{
    public IReadOnlyList<VillaDto> Villas { get; init; } = [];
    public string? Query { get; init; }
    public string? Error { get; init; }
    public bool IsAdmin { get; init; }
}

public sealed class VillaDetailsViewModel
{
    public VillaDto Villa { get; init; } = new();
    public IReadOnlyList<AmenityDto> Amenities { get; init; } = [];
    public bool IsAdmin { get; init; }
}

public sealed class VillaFormViewModel
{
    public int? Id { get; set; }
    [Required, StringLength(30)] public string Name { get; set; } = "";
    [StringLength(200)] public string? Details { get; set; }
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
}

public sealed class LoginViewModel
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
}

public sealed class RegisterViewModel
{
    [Required, StringLength(50)] public string FirstName { get; set; } = "";
    [Required, StringLength(50)] public string LastName { get; set; } = "";
    [Required, StringLength(256)] public string Username { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    [Required, DataType(DataType.Password), Compare(nameof(Password))] public string ConfirmPassword { get; set; } = "";
}
