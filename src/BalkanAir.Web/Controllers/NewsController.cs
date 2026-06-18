namespace BalkanAir.Web.Controllers;

using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class NewsController(INewsService news) : Controller
{
    public async Task<IActionResult> Index()
    {
        var items = await news.GetAll()
            .OrderByDescending(n => n.DateCreated)
            .ToListAsync();
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await news.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }
}
