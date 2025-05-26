using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using E_Learning.Data;
using E_Learning.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Areas.Student.Controllers
{
    [Area("Student")]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // Xem thông tin cá nhân
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var student = await _context.Students
                .Include(s => s.User)  // Nạp thông tin User liên quan
                .FirstOrDefaultAsync(s => s.UserId == currentUser.Id);

            if (student == null)
            {
                // Xử lý khi không tìm thấy student
                return NotFound(); // Hoặc redirect đến trang tạo hồ sơ
            }

            return View(student);  // Truyền model đến View
        }

        // GET: Student/Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Student/Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MSSV,FirstName,LastName")] E_Learning.Models.Student student)
        {
            if (id != student.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
        
   
       

           
          

    }
}