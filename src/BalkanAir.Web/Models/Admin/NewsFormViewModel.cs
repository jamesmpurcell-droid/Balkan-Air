namespace BalkanAir.Web.Models.Admin;

using System.ComponentModel.DataAnnotations;

public class NewsFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(5000)]
    public string Content { get; set; } = string.Empty;

    [Required, Display(Name = "Category")]
    public int CategoryId { get; set; }
}
