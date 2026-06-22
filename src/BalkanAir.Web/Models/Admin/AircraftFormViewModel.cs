namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class AircraftFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Model")]
    public string AircraftModel { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Manufacturer")]
    public int ManufacturerId { get; set; }

    [Range(1, 900)]
    [Display(Name = "Total Seats")]
    public int TotalSeats { get; set; } = 180;
}
