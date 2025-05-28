using E_Learning.Data;
using E_Learning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Learning.Controllers
{
    [Area("Teacher")]
    public class SProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profile/Index
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (teacher == null) return RedirectToAction("Create");

            return View(teacher);
        }

        // GET: Profile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            model.UserId = user.Id;

            // Nếu ImageId không được gán, có thể gán mặc định Guid.Empty hoặc null nếu model hỗ trợ
            if (model.ImageId == Guid.Empty)
            {
                model.ImageId = Guid.NewGuid(); // hoặc để Guid.Empty nếu bạn có xử lý khác
            }

            try
            {
                _context.Teachers.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo hồ sơ giáo viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
                return View(model);
            }
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var teacher = await _context.Teachers.FindAsync(id.Value);
            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();

            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.HireDate = model.HireDate;
            teacher.Status = model.Status;

            if (model.ImageId != Guid.Empty)
            {
                teacher.ImageId = model.ImageId;
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật hồ sơ giáo viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
                return View(model);
            }
        }

        // GET: Profile/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Profile/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword ?? "", model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
    }
}
