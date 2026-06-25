namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class FareFormViewModel
{
    public int Id { get; set; }

    [Required, Range(0.01, 99999.99)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Route")]
    public int RouteId { get; set; }
}
