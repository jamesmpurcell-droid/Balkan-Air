namespace BalkanAir.Web.Models;

using System.ComponentModel.DataAnnotations;
using BalkanAir.Domain.Enums;

public class ProfileViewModel
{
    [StringLength(50)]
    [Display(Name = "First name")]
    public string? FirstName { get; set; }

    [StringLength(50)]
    [Display(Name = "Last name")]
    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of birth")]
    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public string? Nationality { get; set; }

    [Display(Name = "Full address")]
    public string? FullAddress { get; set; }

    [StringLength(20)]
    [Display(Name = "Identity document number")]
    public string? IdentityDocumentNumber { get; set; }
}
