namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class AirportFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(10, MinimumLength = 3), Display(Name = "IATA Code")]
    public string Abbreviation { get; set; } = string.Empty;

    [Required, Display(Name = "Country")]
    public int CountryId { get; set; }
}
