using System.Security.Claims;
using E_Learning.Data;
using E_Learning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Areas.Student.Controllers
{
    [Area("Student")]
    public class DiscussionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscussionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hiển thị danh sách thảo luận
        public async Task<IActionResult> Index(int lessonId)
        {
            var discussions = await _context.Discussions
                .Where(d => d.LessonId == lessonId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            ViewBag.LessonId = lessonId;
            return View(discussions);
        }

        // POST: Gửi bình luận
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(int lessonId, string content)
        {
           

            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null)
            {
                return NotFound(); // Trả lỗi 404 nếu bài học không tồn tại
            }
            if (!string.IsNullOrWhiteSpace(content))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var discussion = new Discussion
                {
                    LessonId = lessonId,
                    Content = content,
                    CreatedAt = DateTime.Now,
                    UserId = userId // hoặc lấy từ UserManager nếu dùng Identity
                };

                _context.Discussions.Add(discussion);
                await _context.SaveChangesAsync();
            }

            // Redirect để tránh form resubmit
            return RedirectToAction(nameof(Index), new { lessonId });
        }
    }
}
