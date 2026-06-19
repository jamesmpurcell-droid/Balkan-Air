namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TravelClassesController(ITravelClassesService travelClasses) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await travelClasses.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var tc = await travelClasses.GetByIdAsync(id);
        return tc is null ? NotFound() : Ok(tc);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TravelClass travelClass)
    {
        var id = await travelClasses.AddAsync(travelClass);
        return CreatedAtAction(nameof(Get), new { id }, travelClass);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await travelClasses.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
