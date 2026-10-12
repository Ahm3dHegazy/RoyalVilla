namespace RoyalVilla_API.DTOs;

public class RegisterDto
{
    [Required]
    [StringLength(50)]
    public string  FirstName { get; set; } 
    
    [Required]
    [StringLength(50)]
    public string  LastName { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(dataType:DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = "Customer";
}