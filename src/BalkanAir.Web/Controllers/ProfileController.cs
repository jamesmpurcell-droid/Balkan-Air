namespace BalkanAir.Web.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ProfileController(UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            Nationality = user.Nationality,
            FullAddress = user.FullAddress,
            IdentityDocumentNumber = user.IdentityDocumentNumber,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.DateOfBirth = model.DateOfBirth;
        user.Gender = model.Gender;
        user.Nationality = model.Nationality;
        user.FullAddress = model.FullAddress;
        user.IdentityDocumentNumber = model.IdentityDocumentNumber;

        await userManager.UpdateAsync(user);
        TempData["Success"] = "Profile updated successfully.";
        return RedirectToAction("Index");
    }
}
