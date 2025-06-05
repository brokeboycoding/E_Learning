using E_Learning.Data;
using E_Learning.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int lessonId)
        {
            var vm = new QuizCreateViewModel { LessonId = lessonId };
           
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuizCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["QuizError"] = string.Join("<br>", errors);

                return View(vm);
            }
            var question = new QuizQuestion
            {
                LessonId = vm.LessonId,
                QuestionText = vm.QuestionText,
                
                Options = vm.Options.Select(o => new QuizOption
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect,
                    Explanation = o.Explanation
                }).ToList()
            };

            _context.QuizQuestions.Add(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageAll", "Course", new { area = "Admin" });
        }
    }
}
