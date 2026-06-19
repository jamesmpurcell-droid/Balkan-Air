namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route = BalkanAir.Domain.Entities.Route;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public class RoutesController(IRoutesService routes) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await routes.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var route = await routes.GetByIdAsync(id);
        return route is null ? NotFound() : Ok(route);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Route route)
    {
        var id = await routes.AddAsync(route);
        return CreatedAtAction(nameof(Get), new { id }, route);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Route route)
    {
        var updated = await routes.UpdateAsync(id, e =>
        {
            e.OriginId = route.OriginId;
            e.DestinationId = route.DestinationId;
            e.DistanceInKm = route.DistanceInKm;
            e.IsDeleted = route.IsDeleted;
        });
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await routes.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
