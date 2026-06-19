namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class FlightsController(ILegInstancesService legInstances) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var flights = await legInstances.GetAll()
            .OrderByDescending(l => l.DepartureDateTime)
            .ToListAsync();
        return Ok(flights);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var flight = await legInstances.GetByIdAsync(id);
        return flight is null ? NotFound() : Ok(flight);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LegInstance flight)
    {
        var id = await legInstances.AddAsync(flight);
        return CreatedAtAction(nameof(Get), new { id }, flight);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] LegInstance flight)
    {
        var updated = await legInstances.UpdateAsync(id, existing =>
        {
            existing.DepartureDateTime = flight.DepartureDateTime;
            existing.ArrivalDateTime = flight.ArrivalDateTime;
            existing.Price = flight.Price;
            existing.FlightLegId = flight.FlightLegId;
            existing.FlightStatusId = flight.FlightStatusId;
            existing.AircraftId = flight.AircraftId;
            existing.IsDeleted = flight.IsDeleted;
        });
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await legInstances.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
