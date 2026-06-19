namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class FaresController(IFaresService fares) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await fares.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var fare = await fares.GetByIdAsync(id);
        return fare is null ? NotFound() : Ok(fare);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Fare fare)
    {
        var id = await fares.AddAsync(fare);
        return CreatedAtAction(nameof(Get), new { id }, fare);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await fares.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
