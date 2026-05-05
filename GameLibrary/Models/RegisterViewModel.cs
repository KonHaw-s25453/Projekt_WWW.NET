using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Potwierdź hasło")]
    [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
