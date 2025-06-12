using E_Learning.Data;
using E_Learning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewStatsController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public ReviewStatsController(ApplicationDbContext context)
        {
            _context = context;


        }
        //Trang hiển thị chung các khóa học để admin dễ dàng chọn khóa học mà mình muốn xem review
        public IActionResult Manage()
        {
            var courses = _context.Courses.Include(c => c.Reviews).ToList(); 
            return View(courses);
        }
        public async Task<IActionResult> ReviewStat(int courseId, int? ratingFilter)
        {
            var course = await _context.Courses
                .Include(c => c.Reviews)
                  
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound();

            var allReviews = course.Reviews.ToList();

            var filteredReviews = allReviews.AsQueryable();
            if (ratingFilter.HasValue)
            {
                filteredReviews = filteredReviews.Where(r => r.Rating == ratingFilter.Value);
            }

            var viewModel = new ReviewStatViewModel
            {
                CourseId = courseId,
                CourseTitle = course.Name,
                AvgRating = allReviews.Any() ? allReviews.Average(r => r.Rating) : 0,
                TotalReviews = allReviews.Count,
                SelectedRating = ratingFilter,
                Reviews = filteredReviews.ToList(),
                RatingGroups = allReviews
                    .GroupBy(r => r.Rating)
                    .Select(g => new RatingGroup
                    {
                        Rating = g.Key,
                        Count = g.Count()
                    })
                    .ToList()
            };

            return View(viewModel);
        }



    }



}

