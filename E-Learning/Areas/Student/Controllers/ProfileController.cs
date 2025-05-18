using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using E_Learning.Data;
using E_Learning.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Areas.Student.Controllers
{
    [Area("Student")]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // Xem thông tin cá nhân
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == currentUser.Id);
            if (student == null)
                return RedirectToAction("CompleteProfile");

            return View(student);
        }

        // GET: Student/Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Student/Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MSSV,FirstName,LastName,EnrollmentDate,Status")] E_Learning.Models.Student student)
        {
            if (id != student.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        // GET: Student/Profile/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Student/Profile/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                // Có thể thêm thông báo thành công tại đây
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        // GET: Student/Profile/CompleteProfile
        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (student == null)
            {
                student = new E_Learning.Models.Student { UserId = user.Id };
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }

            return View(student);
        }

        // POST: Student/Profile/CompleteProfile
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CompleteProfile(E_Learning.Models.Student model, IFormFile? avatarFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            var student = await _context.Students.FindAsync(model.Id);
            if (student == null) return NotFound();

            // Cập nhật dữ liệu từ form
            student.MSSV = model.MSSV;
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                ModelState.AddModelError(nameof(model.FirstName), "Hãy nhập họ");
                return View(model);
            }

            student.LastName = model.LastName;
            student.EnrollmentDate = model.EnrollmentDate;
            student.Status = model.Status;

            // Xử lý upload avatar nếu có
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads", "avatars", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = new FileStream(path, FileMode.Create);
                await avatarFile.CopyToAsync(stream);

                student.ImageId = Guid.Parse(Path.GetFileNameWithoutExtension(fileName));
            }

            _context.Update(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
