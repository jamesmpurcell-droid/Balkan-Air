namespace BalkanAir.Web.Models;

using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
    public required string ConfirmPassword { get; set; }

    [StringLength(50)]
    [Display(Name = "First name")]
    public string? FirstName { get; set; }

    [StringLength(50)]
    [Display(Name = "Last name")]
    public string? LastName { get; set; }
}
