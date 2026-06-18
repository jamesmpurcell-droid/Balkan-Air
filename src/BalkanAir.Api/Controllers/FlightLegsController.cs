namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class FlightLegsController(IFlightLegsService flightLegs) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await flightLegs.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var fl = await flightLegs.GetByIdAsync(id);
        return fl is null ? NotFound() : Ok(fl);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FlightLeg flightLeg)
    {
        var id = await flightLegs.AddAsync(flightLeg);
        return CreatedAtAction(nameof(Get), new { id }, flightLeg);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await flightLegs.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
