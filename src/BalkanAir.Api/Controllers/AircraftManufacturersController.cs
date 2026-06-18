namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AircraftManufacturersController(IAircraftManufacturersService manufacturers) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await manufacturers.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var m = await manufacturers.GetByIdAsync(id);
        return m is null ? NotFound() : Ok(m);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AircraftManufacturer manufacturer)
    {
        var id = await manufacturers.AddAsync(manufacturer);
        return CreatedAtAction(nameof(Get), new { id }, manufacturer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await manufacturers.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
