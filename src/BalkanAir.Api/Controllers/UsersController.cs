namespace BalkanAir.Api.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUsersService users) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await users.GetAll().ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var user = await users.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var user = await users.GetByEmailAsync(email);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await users.SoftDeleteAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
