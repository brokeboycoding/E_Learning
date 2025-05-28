using E_Learning.Data;
using E_Learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Learning.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // Hiển thị thông tin giáo viên hiện tại
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var teacher = await _context.Teachers
                                .Include(t => t.User)
                                .FirstOrDefaultAsync(t => t.UserId == currentUser.Id);

            if (teacher == null)
            {
                // Nếu giáo viên chưa có hồ sơ, điều hướng đến trang tạo mới hồ sơ
                return RedirectToAction(nameof(Create));
            }

            return View(teacher);
        }

        // GET: Teacher/Profile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Profile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,HireDate,Status")] E_Learning.Models.Teacher teacher, IFormFile? imageFile)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            if (ModelState.IsValid)
            {
                teacher.UserId = currentUser.Id;

                if (imageFile != null && imageFile.Length > 0)
                {
                    teacher.ImageId = Guid.NewGuid();

                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "teachers");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, teacher.ImageId + Path.GetExtension(imageFile.FileName));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                }

                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teacher/Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // POST: Teacher/Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,HireDate,Status")] E_Learning.Models.Teacher teacher, IFormFile? imageFile)
        {
            if (id != teacher.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(teacher);

            var teacherInDb = await _context.Teachers.FindAsync(id);
            if (teacherInDb == null)
                return NotFound();

            teacherInDb.FirstName = teacher.FirstName;
            teacherInDb.LastName = teacher.LastName;
            teacherInDb.HireDate = teacher.HireDate;
            teacherInDb.Status = teacher.Status;

            if (imageFile != null && imageFile.Length > 0)
            {
                teacherInDb.ImageId = Guid.NewGuid();

                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "teachers");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, teacherInDb.ImageId + Path.GetExtension(imageFile.FileName));
                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật hồ sơ giáo viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Teacher/Profile/GetImage/{id}
        [HttpGet]
        public IActionResult GetImage(Guid id)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "teachers");

            var files = Directory.GetFiles(uploadsFolder, id.ToString() + ".*");
            if (files.Length == 0)
            {
                return NotFound();
            }

            var filePath = files[0];
            var ext = Path.GetExtension(filePath).ToLower();

            string contentType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
        }

        // GET: Teacher/Profile/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Teacher/Profile/ChangePassword
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
                TempData["Success"] = "Đổi mật khẩu thành công.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
