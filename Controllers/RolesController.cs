using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SashoApp.Models;
using SashoApp.Models.Roles;  // Ако имаш класове, свързани с роли

namespace SashoApp.Controllers
{
    [Authorize(Roles = "Admin")]  // Само администратори могат да използват този контролер
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Показване на всички роли
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();  // Вземаме всички роли
            return View(roles);
        }

        // Формуляр за създаване на нова роля
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Обработване на създаването на нова роля
        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Моля, въведете име на роля.");
                return View();
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));  // Създаване на нова роля
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Ролята вече съществува.");
            return View();
        }

        // Присвояване на роля на потребител
        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = roles;
            return View(user);
        }

        // Обработване на присвояването на роля на потребител
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("", "Ролята не съществува.");
                return View(user);
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);  // Присвояваме ролята
                return RedirectToAction("Index", "Users");  // Пренасочване към списъка с потребители
            }

            ModelState.AddModelError("", "Потребителят вече има тази роля.");
            return View(user);
        }

        // GET: Roles/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string roleName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(roleName))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }

        // GET: Roles/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }
    }
}
