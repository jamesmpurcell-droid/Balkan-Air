namespace BalkanAir.Web.Models;

using System.ComponentModel.DataAnnotations;

public class BookingConfirmViewModel
{
    [Required]
    public int LegInstanceId { get; set; }

    [Required]
    public int TravelClassId { get; set; }

    [Required]
    [Display(Name = "Seat number")]
    public required string SeatNumber { get; set; }

    [Range(1, 99)]
    public int Row { get; set; } = 1;
}
