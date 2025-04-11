using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class UserManagementController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Danh sách tài khoản
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync(); // Sử dụng await với IQueryable
        return View(users);
    }


    // Chỉnh sửa vai trò của người dùng
    public async Task<IActionResult> EditRole(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var roles = await _roleManager.Roles.ToListAsync();
        var userRoles = await _userManager.GetRolesAsync(user) ?? new List<string>(); // Đảm bảo không null

        var model = new EditUserRoleViewModel
        {
            UserId = user.Id ?? string.Empty, // Đảm bảo không null
            Email = user.Email ?? string.Empty, // Đảm bảo không null
            Roles = roles.Select(r => new RoleSelection
            {
                RoleName = r.Name ?? string.Empty, // Đảm bảo không null
                Selected = userRoles.Contains(r.Name ?? string.Empty) // Đảm bảo không lỗi Contains(null)
            }).ToList()
        };

        return View(model);
    }



    [HttpPost]

    public async Task<IActionResult> EditRole(EditUserRoleViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user) ?? new List<string>();
        if (currentRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        var selectedRoles = model.Roles?.Where(r => r.Selected).Select(r => r.RoleName).ToList() ?? new List<string>();
        if (selectedRoles.Any())
        {
            await _userManager.AddToRolesAsync(user, selectedRoles);
        }

        return RedirectToAction("Index");
    }

}
