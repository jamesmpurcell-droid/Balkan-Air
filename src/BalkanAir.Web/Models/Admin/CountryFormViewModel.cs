namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class CountryFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(2, MinimumLength = 2)]
    [Display(Name = "ISO Code (2 chars)")]
    public string Abbreviation { get; set; } = string.Empty;
}
