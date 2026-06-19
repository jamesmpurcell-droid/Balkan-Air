namespace BalkanAir.Services.Contracts;

using BalkanAir.Domain.Entities;

public interface IUsersService : ICrudService<ApplicationUser, string>
{
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task UploadProfilePictureAsync(string userId, byte[] image);
    Task SetLastLoginAsync(string userEmail, DateTime dateTime);
    Task SetLastLogoutAsync(string userEmail, DateTime dateTime);
    Task SetLogoffForUserAsync(string userId, bool logoff);
}
