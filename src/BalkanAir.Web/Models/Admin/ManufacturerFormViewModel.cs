namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class ManufacturerFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
}
