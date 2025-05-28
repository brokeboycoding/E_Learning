using System.Security.Claims;
using E_Learning.Data;
using E_Learning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Areas.Student.Controllers
{
    [Area("Student")]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;
      
   

        public LessonController(ApplicationDbContext context )
        {
            _context = context;
       
            
        }

        // GET: Xem module & bài học đang chọn
        public async Task<IActionResult> Module(int? lessonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lấy danh sách modules và include cả Course
            var modules = await _context.Modules
                .Include(m => m.Lessons)
                .Include(m => m.Course) // Thêm include này
                .ToListAsync();

            Lesson? selectedLesson = null;

            if (lessonId.HasValue)
            {
                selectedLesson = await _context.Lessons
                    .Include(l => l.Module) // Include Module để lấy CourseId
                    .ThenInclude(m => m!.Course)
                    .FirstOrDefaultAsync(l => l.Id == lessonId.Value);
                HttpContext.Session.SetInt32("LastLessonId", lessonId.Value);
            }
            else
            {
                lessonId = HttpContext.Session.GetInt32("LastLessonId");
            }

            var notes = new List<LessonNote>();
            if (selectedLesson != null)
            {
                notes = await _context.LessonNotes
               .Include(n => n.User) // Nếu cần hiển thị thông tin người tạo
               .Where(n => n.LessonId == selectedLesson.Id && n.UserId == userId)
               .OrderByDescending(n => n.Timestamp) // Sắp xếp theo thời gian
               .ToListAsync();
            }

            // Xác định Course hiện tại
            Course? currentCourse = null;
            if (selectedLesson != null)
            {
                currentCourse = selectedLesson.Module?.Course;
            }
            else if (modules.Any())
            {
                currentCourse = modules.First().Course;
            }

            var vm = new CourseViewModel
            {
                Modules = modules,
                CurrentLesson = selectedLesson,
                CurrentCourse = currentCourse, // Thêm dòng này
                Notes = notes,
                NewNote = new LessonNote { LessonId = lessonId ?? 0 }
            };

            var completedLessons = await _context.lessonProgresses
                .Where(p => p.UserId == userId && p.IsCompleted)
                .Select(p => p.LessonId)
                .ToListAsync();

            ViewBag.CompletedLessons = completedLessons;

            // Kiểm tra đăng ký khóa học
            if (currentCourse != null)
            {
                var isEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.UserId == userId && e.CourseId == currentCourse.Id);
                ViewBag.IsEnrolled = isEnrolled;
            }
            else
            {
                ViewBag.IsEnrolled = false;
            }

            return View(vm);
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exists = await _context.Enrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (!exists)
            {
                var enrollment = new Enrollment
                {
                    UserId = userId!,
                    CourseId = courseId
                };
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng ký khóa học thành công!";
            }
            else
            {
                TempData["Warning"] = "Bạn đã đăng ký khóa học này!";
            }

            return RedirectToAction("Module", new { id = courseId });
        }
        //Khoa hoc dang hoc cua sv do
        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var courses = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.Course)
                .Select(e => e.Course)
                .ToListAsync();

            return View(courses);
        }
        public async Task<IActionResult> CompleteLesson(int lessonId)
        {
          
            var lesson = await _context.Lessons.FindAsync(lessonId);

            if (lesson == null ) return NotFound();

            
            await _context.SaveChangesAsync();
          

           
            return RedirectToAction("Module", new { lessonId = lesson.Id });

        }
        //Ranking nhe
    





        // AJAX: Load chi tiết bài học
        public async Task<IActionResult> LoadLesson(int id)
        {
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
                return NotFound();

            return PartialView("_LessonDetailPartial", lesson);
        }
        [HttpPost]
        public async Task<IActionResult> MarkComplete(int lessonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var progress = await _context.lessonProgresses
                .FirstOrDefaultAsync(p => p.LessonId == lessonId && p.UserId == userId);

            if (progress == null)
            {
                progress = new LessonProgress
                {
                    LessonId = lessonId,
                    UserId = userId!,
                    IsCompleted = true
                };
                _context.lessonProgresses.Add(progress);
            }
            else
            {
                progress.IsCompleted = true;
                _context.lessonProgresses.Update(progress);
            }

            await _context.SaveChangesAsync();
           
            TempData["Success"] = "Đã đánh dấu hoàn thành bài học!";
            
            return RedirectToAction("Module", new { lessonId });
        }



        [HttpPost]
       
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(LessonNote note)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Kiểm tra validation
            if (!ModelState.IsValid)
            {
                // Nếu không hợp lệ, trả về view với thông báo lỗi
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin ghi chú";
                return RedirectToAction("Module", new { lessonId = note.LessonId });
            }

            note.UserId = userId;

            try
            {
                _context.LessonNotes.Add(note);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã lưu ghi chú thành công!";
            }
            catch (DbUpdateException ex)
            {
               
                TempData["Error"] = "Lỗi khi lưu ghi chú: " + ex.Message;
            }

            return RedirectToAction("Module", new { lessonId = note.LessonId });
        }


        // POST: Xóa ghi chú
        [HttpPost]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.LessonNotes.FindAsync(id);
            if (note == null)
                return NotFound();

            int lessonId = note.LessonId;

            _context.LessonNotes.Remove(note);
            await _context.SaveChangesAsync();

            return RedirectToAction("Module", new { lessonId });
        }


    }
}
