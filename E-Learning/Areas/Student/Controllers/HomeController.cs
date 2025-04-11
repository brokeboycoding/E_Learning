using System.Diagnostics;
using E_Learning.Areas.Admin.Controllers;
using E_Learning.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Areas.Student.Controllers
{
    [Area("Student")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICourseService _courseService;
        public HomeController(ILogger<HomeController> logger,ICourseService courseService)
        {
            _logger = logger;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(string keyword,string type)
        {
            var courses = await _courseService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                courses = courses.Where(x => x.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            ViewData["Keyword"] = keyword;
         
            return View(courses);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
