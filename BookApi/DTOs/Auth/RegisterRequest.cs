using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookApi.DTOs.Auth;

public record RegisterRequest
{
    public required string Username {get; set;}

    public required string Email {get; set;}

    [Required(ErrorMessage = "Password is required. ")]
    [MinLength(8, ErrorMessage = "Password must be min 8 characters long. ")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$", 
    ErrorMessage = "Password must contain one uppercase letter, one lowercase letter and one number. ")]
    public required string Password {get; set;}
}

