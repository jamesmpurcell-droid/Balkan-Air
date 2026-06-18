namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AircraftsController(IAircraftsService aircrafts) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await aircrafts.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var aircraft = await aircrafts.GetByIdAsync(id);
        return aircraft is null ? NotFound() : Ok(aircraft);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Aircraft aircraft)
    {
        var id = await aircrafts.AddAsync(aircraft);
        return CreatedAtAction(nameof(Get), new { id }, aircraft);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Aircraft aircraft)
    {
        var updated = await aircrafts.UpdateAsync(id, e =>
        {
            e.Model = aircraft.Model;
            e.TotalSeats = aircraft.TotalSeats;
            e.AircraftManufacturerId = aircraft.AircraftManufacturerId;
            e.IsDeleted = aircraft.IsDeleted;
        });
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await aircrafts.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
