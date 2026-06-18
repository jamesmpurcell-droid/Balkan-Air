namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class NewsController(INewsService news) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await news.GetAll().OrderByDescending(n => n.DateCreated).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await news.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] News item)
    {
        var id = await news.AddAsync(item);
        return CreatedAtAction(nameof(Get), new { id }, item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await news.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
