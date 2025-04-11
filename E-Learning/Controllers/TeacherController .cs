using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_Learning.Models;

[Authorize(Roles = "Teacher")]
public class TeacherController : Controller
{
    public IActionResult AddLesson()
    {
        // Logic để thêm bài giảng
        return View();
    }

    // Các action khác cho Teacher
}
