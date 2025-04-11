using E_Learning.Models;

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Learning.Areas.Admin.Controllers
{
    [Area("Admin")] //Quyền admin
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // Hiển thị danh sách khóa học
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }

        // GET: Admin/Course/Create
        public IActionResult Create()
        {
            return View(new Course());
        }

        // Tạo khóa học mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _courseService.CreateAsync(model);
            TempData["Success"] = "Thêm khoá học thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra khóa học
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            // nếu khóa học không tồn tại
            if (course == null)
            {
                TempData["Error"] = "Khoá học không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // Sửa khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _courseService.UpdateAsync(model);
            if (!success)
            {
                TempData["Error"] = "Cập nhật thất bại!";
                return View(model);
            }

            TempData["Success"] = "Cập nhật thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra khóa học
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                TempData["Error"] = "Khoá học không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // Xóa khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _courseService.DeleteAsync(id);
            if (!success)
            {
                TempData["Error"] = "Xoá thất bại!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Xoá thành công!";
            return RedirectToAction(nameof(Index));
        }
    }

    public interface ICourseService
    {
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<bool> CreateAsync(Course course);
        Task<bool> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
    }
}
