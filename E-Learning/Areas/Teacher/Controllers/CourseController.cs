using E_Learning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_Learning.Services;
using E_Learning.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http; // để dùng IFormFile
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static E_Learning.Models.CourseModuleViewModel;

namespace E_Learning.Areas.Teacher.Controllers
{
    [Area("Teacher")] // Quyền Teacher
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public CourseController(ICourseService courseService, ApplicationDbContext context, IWebHostEnvironment env, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _context = context;
            _env = env;
            _logger = logger;
        }

        // Hiển thị danh sách khóa học
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }

        // GET: Teacher/Course/Create
        [HttpGet]
        public IActionResult Create(int? moduleCount)
        {
            int count = moduleCount ?? 1; // Mặc định 1 module

            var model = new CourseModuleViewModel
            {
                Modules = new List<ModuleViewModel>()
            };

            for (int i = 0; i < count; i++)
            {
                model.Modules.Add(new ModuleViewModel());
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseModuleViewModel vm)
        {
            var validModules = vm.Modules?
                .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                .ToList() ?? new List<ModuleViewModel>();

            if (!ModelState.IsValid || validModules.Count == 0)
            {
                if (validModules.Count == 0)
                {
                    ModelState.AddModelError("", "Khóa học phải có ít nhất một module hợp lệ.");
                }

                vm.Modules = vm.Modules?.Count > 0 ? vm.Modules : new List<ModuleViewModel> { new ModuleViewModel() };
                return View(vm);
            }

            try
            {
                var course = new Course
                {
                    Name = vm.Name.Trim(),
                    Description = vm.Description?.Trim(),
                    Duration = vm.Duration,
                    IsActive = vm.IsActive,
                    Modules = validModules.Select(m => new Module
                    {
                        Name = m.Name.Trim()
                    }).ToList()
                };

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Tạo khóa học thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course");

                TempData["Error"] = "Đã xảy ra lỗi khi tạo khóa học.";
                vm.Modules = vm.Modules ?? new List<ModuleViewModel> { new ModuleViewModel() };
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int? moduleCount = null)
        {
            var course = await _context.Courses
                .Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            var currentModules = course.Modules
                .Select(m => new ModuleViewModel
                {
                    Id = m.Id,
                    Name = m.Name ?? "Tên mặc định"
                }).ToList();

            int totalModules = moduleCount ?? currentModules.Count;

            while (currentModules.Count < totalModules)
                currentModules.Add(new ModuleViewModel());

            var vm = new CourseModuleViewModel
            {
                Id = course.Id,
                Name = course.Name ?? "Tên mặc định",
                Description = course.Description ?? "",
                Duration = course.Duration,
                IsActive = course.IsActive,
                Modules = currentModules
            };

            ViewBag.ModuleCountList = ViewBag.ModuleCountList ?? Enumerable.Range(1, 20).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseModuleViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (vm.Modules == null || vm.Modules.Count == 0)
                {
                    vm.Modules = new List<ModuleViewModel> { new ModuleViewModel() };
                }
                return View(vm);
            }

            try
            {
                var course = await _context.Courses
                    .Include(c => c.Modules)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (course == null)
                {
                    return NotFound();
                }

                course.Name = vm.Name.Trim();
                course.Description = vm.Description?.Trim();
                course.Duration = vm.Duration;
                course.IsActive = vm.IsActive;

                _context.Modules.RemoveRange(course.Modules);

                var validModules = vm.Modules?
                    .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                    .Select(m => new Module
                    {
                        Name = m.Name.Trim(),
                        CourseId = course.Id
                    }).ToList() ?? new List<Module>();

                if (validModules.Count == 0)
                {
                    ModelState.AddModelError("", "Khóa học phải có ít nhất một module");
                    vm.Modules = new List<ModuleViewModel> { new ModuleViewModel() };
                    return View(vm);
                }

                course.Modules = validModules;

                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật khóa học thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật khóa học");
                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật khóa học";
                vm.Modules = vm.Modules ?? new List<ModuleViewModel> { new ModuleViewModel() };
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                TempData["Error"] = "Khoá học không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                TempData["Error"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }

            _context.Modules.RemoveRange(course.Modules);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xoá khóa học và các module liên quan.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageAll()
        {
            var courses = _context.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .ToList();

            return View(courses);
        }

        public IActionResult Manage(int courseId)
        {
            var course = _context.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> CreateLesson(int moduleId)
        {
            var module = await _context.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.Id == moduleId);
            if (module == null)
            {
                TempData["Error"] = "Module không tồn tại";
                return RedirectToAction("ManageAll");
            }

            ViewBag.ModuleTitle = module.Name;
            ViewBag.CourseId = module.CourseId;

            var lesson = new Lesson { ModuleId = module.Id };

            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLesson(Lesson lesson, IFormFile VideoFile)
        {
            if (lesson == null)
            {
                TempData["Error"] = "Dữ liệu bài học không hợp lệ.";
                return RedirectToAction("ManageAll");
            }

            // Kiểm tra ModuleId hợp lệ
            if (lesson.ModuleId <= 0)
            {
                ModelState.AddModelError("", "ModuleId không hợp lệ.");
            }

            // Lấy module 1 lần, tránh gọi nhiều lần
            var module = await _context.Modules.Include(m => m.Course).FirstOrDefaultAsync(m => m.Id == lesson.ModuleId);
            if (module == null)
            {
                TempData["Error"] = "Module không tồn tại.";
                return RedirectToAction("ManageAll");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModuleTitle = module.Name;
                ViewBag.CourseId = module.CourseId;
                return View(lesson);
            }

            try
            {
                if (VideoFile != null && VideoFile.Length > 0)
                {
                    // Đường dẫn thư mục lưu video, ví dụ wwwroot/videos/lesson
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "videos", "lesson");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Tên file an toàn tránh trùng lặp
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoFile.FileName);

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await VideoFile.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn tương đối vào database (ví dụ: /videos/lesson/filename.mp4)
                    lesson.VideoUrl = $"/videos/lesson/{uniqueFileName}";
                }

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thêm bài học thành công.";
                return RedirectToAction("Manage", new { courseId = module.CourseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi thêm bài học");
                TempData["Error"] = "Lỗi khi thêm bài học.";
                ViewBag.ModuleTitle = module.Name;
                ViewBag.CourseId = module.CourseId;
                return View(lesson);
            }
        }
    }
}
