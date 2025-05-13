//using E_Learning.Data;
//using E_Learning.Models;
//using Microsoft.EntityFrameworkCore;

//namespace E_Learning.Areas.Admin.Controllers
//{
//    public class CourseService : ICourseService
//    {
//        private readonly ApplicationDbContext _context;

//        public CourseService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Course?> GetByIdAsync(int id)
//        {
//            return await _context.Courses.FindAsync(id);
//        }

//        public async Task<IEnumerable<Course>> GetAllAsync()
//        {
//            return await _context.Courses.ToListAsync();
//        }

//        public async Task<bool> CreateAsync(Course course)
//        {
//            try
//            {
//                await _context.Courses.AddAsync(course);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public async Task<bool> UpdateAsync(Course course)
//        {
//            try
//            {
//                _context.Courses.Update(course);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public async Task<bool> DeleteAsync(int id)
//        {
//            try
//            {
//                var course = await GetByIdAsync(id);
//                if (course == null) return false;

//                _context.Courses.Remove(course);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}

