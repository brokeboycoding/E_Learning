using E_Learning.Data;
using E_Learning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Learning.Controllers
{
    [Area("Student")]
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

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (student == null) return RedirectToAction("Create");

            return View(student);

        }

        // GET: Profile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student model)
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
                _context.Students.Add(model);
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

            var student = await _context.Students.FindAsync(id.Value);
            if (student == null)
                return NotFound();

            return View(student);
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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword ?? "", model.NewPassword);
            if (result.Succeeded)
            {
                // Đổi mật khẩu thành công => chuyển đến trang hỏi đăng nhập lại
                return RedirectToAction(nameof(ChangePasswordSuccess));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public IActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
