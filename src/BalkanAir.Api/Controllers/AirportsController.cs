namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AirportsController(IAirportsService airports) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await airports.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var airport = await airports.GetByIdAsync(id);
        return airport is null ? NotFound() : Ok(airport);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Airport airport)
    {
        var id = await airports.AddAsync(airport);
        return CreatedAtAction(nameof(Get), new { id }, airport);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Airport airport)
    {
        var updated = await airports.UpdateAsync(id, e =>
        {
            e.Name = airport.Name;
            e.Abbreviation = airport.Abbreviation;
            e.CountryId = airport.CountryId;
            e.IsDeleted = airport.IsDeleted;
        });
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await airports.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
