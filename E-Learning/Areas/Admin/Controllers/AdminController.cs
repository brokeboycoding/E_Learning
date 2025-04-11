using E_Learning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

    [Authorize(Roles = "Admin")] // Chỉ Admin mới được truy cập
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Hiển thị danh sách tài khoản Teacher & Student
        public async Task<IActionResult> ManageAccounts()
        {
            var teacherUsers = await _userManager.GetUsersInRoleAsync("Teacher");
            var studentUsers = await _userManager.GetUsersInRoleAsync("Student");

            var users = teacherUsers.Concat(studentUsers).ToList(); // Gộp hai danh sách

            // Lấy vai trò của mỗi người dùng và truyền vào ViewBag
            var userRoles = new Dictionary<string, List<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.ToList();
            }

            ViewBag.UserRoles = userRoles; // Truyền thông tin vai trò vào ViewBag

            return View(users);
        }


        // Hiển thị form tạo tài khoản mới
        public IActionResult CreateAccount()
        {
            return View();
        }

        // Xử lý tạo tài khoản mới
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

                // Nếu là Student, dùng MSSV làm username, còn nếu không thì dùng Email
                string username = model.Role == "Student" ? model.Identifier : model.Email;
                string email = model.Role == "Student" ? $"{model.Identifier}@example.com" : model.Email!;

                // Kiểm tra nếu UserName hoặc Email đã tồn tại
                var existingUserByUsername = await _userManager.FindByNameAsync(username);
                var existingUserByEmail = await _userManager.FindByEmailAsync(email);

                if (existingUserByUsername != null || existingUserByEmail != null)
                {
                    ModelState.AddModelError("", "MSSV hoặc Email đã tồn tại.");
                    return View(model);
                }

                // Tạo tài khoản mới
                var user = new IdentityUser { UserName = username, Email = email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    // Kiểm tra Role có tồn tại không, nếu chưa có thì tạo
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    // Thêm người dùng vào vai trò đã chọn
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("ManageAccounts");
                }
                else
                {
                    // Hiển thị lỗi nếu tạo tài khoản thất bại
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        // Xóa tài khoản
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
