﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SashoApp.Models;
using SashoApp.Models.Account;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SashoApp.Controllers
{
public class AccountController : Controller
{
private readonly UserManager<ApplicationUser> _userManager;
private readonly SignInManager<ApplicationUser> _signInManager;
private readonly RoleManager<IdentityRole> _roleManager;


    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    // List users with roles
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ListUsers()
    {
        var users = _userManager.Users.ToList();
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                FamilyName = user.FamilyName,
                Roles = roles
            });
        }

        return View(userViewModels);
    }

    // Регистрация
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                FamilyName = model.FamilyName,
                PasswordHash = model.Password,
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync("User"))
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Car");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // Вход
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Car");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
        }

        return View(model);
    }

    // Изход
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // Промяна на ролята (например, за администраторите)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PromoteToManager(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.AddToRoleAsync(user, "Manager");
        if (result.Succeeded)
        {
            return RedirectToAction("ListUsers");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("ListUsers");
    }

    // Премахване на ролята "Manager"
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DemoteToUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.RemoveFromRoleAsync(user, "Manager");
        if (result.Succeeded)
        {
            return RedirectToAction("ListUsers");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("ListUsers");
    }

    // Promote user to Admin role
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PromoteToAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.AddToRoleAsync(user, "Admin");
        if (result.Succeeded)
        {
            return RedirectToAction("ListUsers");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("ListUsers");
    }

    // Demote user from Admin role
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DemoteFromAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
        if (result.Succeeded)
        {
            return RedirectToAction("ListUsers");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToAction("ListUsers");
    }
}
}