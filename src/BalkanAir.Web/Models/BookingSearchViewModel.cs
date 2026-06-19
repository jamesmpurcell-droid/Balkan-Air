namespace BalkanAir.Web.Models;

using System.ComponentModel.DataAnnotations;

public class BookingSearchViewModel
{
    [Required]
    [Display(Name = "From")]
    public int? DepartureAirportId { get; set; }

    [Required]
    [Display(Name = "To")]
    public int? DestinationAirportId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Departure date")]
    public DateTime? DepartureDate { get; set; }

    [Range(1, 9)]
    public int Passengers { get; set; } = 1;
}
