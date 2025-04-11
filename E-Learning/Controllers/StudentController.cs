using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    public IActionResult Index()
    {
        // Trang chủ cho Student
        return View();
    }

    // Các action khác cho Student nếu cần
}
