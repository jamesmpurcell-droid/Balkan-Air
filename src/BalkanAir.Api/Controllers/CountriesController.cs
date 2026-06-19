namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CountriesController(ICountriesService countries) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await countries.GetAll().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var country = await countries.GetByIdAsync(id);
        return country is null ? NotFound() : Ok(country);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Country country)
    {
        var id = await countries.AddAsync(country);
        return CreatedAtAction(nameof(Get), new { id }, country);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Country country)
    {
        var updated = await countries.UpdateAsync(id, e =>
        {
            e.Name = country.Name;
            e.Abbreviation = country.Abbreviation;
            e.IsDeleted = country.IsDeleted;
        });
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await countries.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
