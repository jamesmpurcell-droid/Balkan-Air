namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;
using BalkanAir.Domain.Enums;

public class TravelClassFormViewModel
{
    public int Id { get; set; }

    [Required]
    public TravelClassType Type { get; set; }

    [Required, StringLength(200)]
    public string Meal { get; set; } = string.Empty;

    [Display(Name = "Priority Boarding")]
    public bool PriorityBoarding { get; set; }

    [Display(Name = "Reserved Seat")]
    public bool ReservedSeat { get; set; } = true;

    [Display(Name = "Earn Miles")]
    public bool EarnMiles { get; set; }

    [Range(1, 100)]
    [Display(Name = "Number of Rows")]
    public int NumberOfRows { get; set; } = 10;

    [Range(1, 10)]
    [Display(Name = "Seats per Row")]
    public int NumberOfSeats { get; set; } = 6;

    [Range(0, 99999.99)]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Aircraft")]
    public int AircraftId { get; set; }
}
