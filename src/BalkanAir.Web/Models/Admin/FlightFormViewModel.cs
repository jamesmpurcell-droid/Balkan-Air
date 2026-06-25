namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class FlightFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(10)]
    [Display(Name = "Flight Number")]
    public string Number { get; set; } = string.Empty;
}
