using E_Learning.Models;

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_Learning.Services;
using static E_Learning.Models.CourseModuleViewModel;
using E_Learning.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Areas.Admin.Controllers
{
    [Area("Admin")] //Quyền admin
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;
        public CourseController(ICourseService courseService, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _courseService = courseService;
            _context = context;
            _env = env;
        }

        // Hiển thị danh sách khóa học
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }

        // GET: Admin/Course/Create
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
            // Lọc các module có tên hợp lệ
            var validModules = vm.Modules?
                .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                .ToList() ?? new List<ModuleViewModel>();

            if (!ModelState.IsValid || validModules.Count == 0)
            {
                if (validModules.Count == 0)
                {
                    ModelState.AddModelError("", "Khóa học phải có ít nhất một module hợp lệ.");
                }

                // Gán lại danh sách module đã nhập (giữ lại dữ liệu người dùng nhập)
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
                    Name = m.Name
                }).ToList();

            // Nếu không có giá trị moduleCount, lấy số module hiện tại
            int totalModules = moduleCount ?? currentModules.Count;

            // Nếu moduleCount lớn hơn số module hiện tại, thêm module trống
            while (currentModules.Count < totalModules)
                currentModules.Add(new ModuleViewModel());

            var vm = new CourseModuleViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Duration = course.Duration,
                IsActive = course.IsActive,
                Modules = currentModules
            };

            // Khởi tạo ViewBag.ModuleCountList nếu chưa có
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

                // Cập nhật course
                course.Name = vm.Name.Trim();
                course.Description = vm.Description?.Trim();
                course.Duration = vm.Duration;
                course.IsActive = vm.IsActive;

                // Xóa các module cũ
                _context.Modules.RemoveRange(course.Modules);

                // Thêm lại modules mới hợp lệ
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








        // Kiểm tra khóa học
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

            return View(course); // View hiển thị thêm danh sách Module nếu muốn
        }

        // Xóa khóa học
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

            _context.Modules.RemoveRange(course.Modules); // xoá module trước (EF Cascade cũng được)
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xoá khóa học và các module liên quan.";
            return RedirectToAction(nameof(Index));
        }
        // Quản lý bài học trong khóa học
        public IActionResult ManageAll()
        {
            var courses = _context.Courses
                .Include(c => c.Modules)
                    .ThenInclude(m => m.Lessons)
                .ToList();

            return View(courses);
        }
        //Quản lý riêng cho từng khóa học
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


        // GET: Tạo bài giảng
        [HttpGet]
        // Ví dụ khi GET CreateLesson
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

            // Gán ModuleId vào model
            var lesson = new Lesson { ModuleId = module.Id };

            return View(lesson);
        }


        // POST: Tạo bài giảng

        [HttpPost]
        public async Task<IActionResult> CreateLesson(Lesson lesson, IFormFile VideoFile)
        {
            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    var key = kvp.Key;
                    var errors = kvp.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"ModelState Error at '{key}': {error.ErrorMessage}");
                    }
                }

                TempData["Error"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";

                var moduleForError = await _context.Modules
                    .Include(m => m.Course)
                    .FirstOrDefaultAsync(m => m.Id == lesson.ModuleId);

                if (moduleForError != null)
                {
                    ViewBag.ModuleTitle = moduleForError.Name;
                    ViewBag.CourseId = moduleForError.CourseId;
                }

                return View(lesson);
            }

            try
            {
                // Upload video nếu có
                if (VideoFile != null && VideoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await VideoFile.CopyToAsync(stream);
                    }

                    lesson.VideoUrl = "/videos/" + fileName;
                }

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Tạo bài học thành công!";

                var module = await _context.Modules
                    .Include(m => m.Course)
                    .FirstOrDefaultAsync(m => m.Id == lesson.ModuleId);

                if (module?.CourseId == null)
                {
                    TempData["Error"] = "Không tìm thấy khóa học liên quan.";
                    return RedirectToAction("ManageAll");
                }

                return RedirectToAction("Manage", new { courseId = module.CourseId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
                Console.WriteLine("STACK TRACE: " + ex.StackTrace);

                ModelState.AddModelError("", "Lỗi khi tạo bài học: " + ex.Message);
                TempData["Error"] = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại.";

                var moduleForError = await _context.Modules
                    .Include(m => m.Course)
                    .FirstOrDefaultAsync(m => m.Id == lesson.ModuleId);

                if (moduleForError != null)
                {
                    ViewBag.ModuleTitle = moduleForError.Name;
                    ViewBag.CourseId = moduleForError.CourseId;
                }

                return View(lesson);
            }
        }



        // GET: Sửa bài học
        [HttpGet]
        public async Task<IActionResult> EditLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                TempData["Error"] = "Bài học không tồn tại.";
                return RedirectToAction("Manage");
            }

            ViewBag.Modules = new SelectList(_context.Modules, "Id", "Name", lesson.ModuleId);

            return View("CreateLesson", lesson);
        }

        // POST: Sửa bài học
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLesson(Lesson lesson)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Modules = new SelectList(_context.Modules, "Id", "Name", lesson.ModuleId);
                return View("CreateLesson", lesson);
            }

            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();

            var courseId = _context.Modules.FirstOrDefault(m => m.Id == lesson.ModuleId)?.CourseId;
            TempData["Success"] = "Cập nhật bài học thành công!";
            return RedirectToAction("Manage", new { courseId });
        }

        // Xoá bài học
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Module)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
            {
                TempData["Error"] = "Bài học không tồn tại.";
                return RedirectToAction("Manage");
            }

            var courseId = lesson.Module.CourseId;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xoá bài học.";
            return RedirectToAction("Manage", new { courseId });
        }

    }


    //public interface ICourseService
    //{
    //    Task<Course?> GetByIdAsync(int id);
    //    Task<IEnumerable<Course>> GetAllAsync();
    //    Task<bool> CreateAsync(Course course);
    //    Task<bool> UpdateAsync(Course course);
    //    Task<bool> DeleteAsync(int id);
    //}

}