using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using E_Learning.Models;

namespace E_Learning.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageAccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageAccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: Admin/ManageAccount
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManageAccounts()
        {
            var teacherUsers = await _userManager.GetUsersInRoleAsync("Teacher");
            var studentUsers = await _userManager.GetUsersInRoleAsync("Student");
            var users = teacherUsers.Concat(studentUsers).ToList();

            var userRoles = new Dictionary<string, List<string>>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.ToList();
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }
        // GET: Admin/ManageAccount/CreateAccount
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var validRoles = new[] { "Admin", "Teacher", "Student" };
                if (!validRoles.Contains(model.Role))
                {
                    ModelState.AddModelError("Role", "Vai trò không hợp lệ.");
                    return View(model);
                }

                string username = model.Role == "Student" ? model.Identifier : model.Email;
                string email = model.Role == "Student" ? $"{model.Identifier}@example.com" : model.Email!;

                var existingUserByUsername = await _userManager.FindByNameAsync(username);
                var existingUserByEmail = await _userManager.FindByEmailAsync(email);

                if (existingUserByUsername != null || existingUserByEmail != null)
                {
                    ModelState.AddModelError("", "MSSV hoặc Email đã tồn tại.");
                    return View(model);
                }

                var user = new IdentityUser { UserName = username, Email = email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("ManageAccounts");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }
        
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    foreach (var role in roles)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("ManageAccounts");
        }
    }
}
