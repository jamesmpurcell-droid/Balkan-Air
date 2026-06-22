namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class RouteFormViewModel
{
    public int Id { get; set; }

    [Required, Display(Name = "Origin Airport")]
    public int OriginId { get; set; }

    [Required, Display(Name = "Destination Airport")]
    public int DestinationId { get; set; }

    [Range(0, 50000), Display(Name = "Distance (km)")]
    public double DistanceInKm { get; set; }

    [Range(0, 99999.99), Display(Name = "Base Fare Price")]
    public decimal FarePrice { get; set; }
}
