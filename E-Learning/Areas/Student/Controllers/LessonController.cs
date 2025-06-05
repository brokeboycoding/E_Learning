using System.Reflection;
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
        public async Task<IActionResult> Module(int? lessonId, int? courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Lesson? selectedLesson = null;

            // 1. Ưu tiên lấy lesson theo lessonId
            if (lessonId.HasValue)
            {
                selectedLesson = await _context.Lessons
                    .Include(l => l.Module)
                        .ThenInclude(m => m.Course)
                        .Include(l => l.QuizQuestions)
                        .ThenInclude(q => q.Options)
                    .FirstOrDefaultAsync(l => l.Id == lessonId.Value);

                if (selectedLesson != null)
                {
                    HttpContext.Session.SetInt32("LastLessonId", selectedLesson.Id);
                    courseId = selectedLesson.Module?.CourseId;
                }
            }
            // 2. Nếu không có lessonId, lấy từ Session
            else if (HttpContext.Session.GetInt32("LastLessonId") is int lastLessonId)
            {
                selectedLesson = await _context.Lessons
                    .Include(l => l.Module)
                        .ThenInclude(m => m.Course)
                         .Include(l => l.QuizQuestions)
                        .ThenInclude(q => q.Options)
                    .FirstOrDefaultAsync(l => l.Id == lastLessonId);

                if (selectedLesson != null)
                {
                    courseId = selectedLesson.Module?.CourseId;
                }
            }

            // 3. Nếu vẫn chưa có lesson, nhưng có courseId => lấy bài học đầu tiên
            if (selectedLesson == null && courseId.HasValue)
            {
                selectedLesson = await _context.Modules
                    .Where(m => m.CourseId == courseId.Value)
                    .Include(m => m.Lessons)
                        .ThenInclude(l => l.Module)
                            .ThenInclude(m => m.Course)
                    .SelectMany(m => m.Lessons)
                    .FirstOrDefaultAsync();

                if (selectedLesson != null)
                {
                    HttpContext.Session.SetInt32("LastLessonId", selectedLesson.Id);
                    courseId = selectedLesson.Module?.CourseId;
                }
            }

            // 4. Nếu vẫn null, trả về NotFound hoặc trang trắng
            if (selectedLesson == null)
            {
                return NotFound("Không tìm thấy bài học hoặc khoá học.");
            }

            // 5. Lấy danh sách module thuộc khoá học
            var modules = await _context.Modules
                .Where(m => m.CourseId == courseId)
                .Include(m => m.Lessons)
                .ToListAsync();

            // 6. Ghi chú
            var notes = await _context.LessonNotes
                .Include(n => n.User)
                .Where(n => n.LessonId == selectedLesson.Id && n.UserId == userId)
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();

            // 7. Giao diện ViewModel
            var vm = new CourseViewModel
            {
                Modules = modules,
                CurrentLesson = selectedLesson,
                CurrentCourse = selectedLesson.Module?.Course,
                Notes = notes,
                NewNote = new LessonNote { LessonId = selectedLesson.Id }
            };

            // 8. Bài học đã hoàn thành
            ViewBag.CompletedLessons = await _context.lessonProgresses
                .Where(p => p.UserId == userId && p.IsCompleted)
                .Select(p => p.LessonId)
                .ToListAsync();

            // 9. Kiểm tra đã đăng ký khoá học chưa
            ViewBag.IsEnrolled = await _context.Enrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == selectedLesson.Module.CourseId);

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
                    UserId = userId,
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
        //trac nghiem nhe
     






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
                    UserId = userId,
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
        public async Task<IActionResult> AddNoteAjax([FromForm] LessonNote note)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Người dùng chưa đăng nhập." });
            note.UserId = userId;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            

            try
            {
                _context.LessonNotes.Add(note);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    note = new
                    {
                        id = note.Id,
                        content = note.Content,
                        timestamp = note.Timestamp.ToString("0.0")
                    }
                });
            }
            catch (DbUpdateException ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu ghi chú: " + ex.Message });
            }
        }
        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNoteAjax([FromForm] LessonNote note)
        {
            var existing = await _context.LessonNotes.FindAsync(note.Id);
            if (existing == null)
                return Json(new { success = false, message = "Ghi chú không tồn tại." });

            existing.Content = note.Content;
            existing.Timestamp = note.Timestamp;
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                note = new
                {
                    id = existing.Id,
                    content = existing.Content,
                    timestamp = existing.Timestamp
                }
            });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteNoteAjax(int id)
        {
            var note = await _context.LessonNotes.FindAsync(id);
            if (note == null)
                return Json(new { success = false, message = "Không tìm thấy ghi chú." });

            _context.LessonNotes.Remove(note);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }



    }
}
