//using System;
//using E_Learning.Data;
//using E_Learning.Models;
//using Microsoft.EntityFrameworkCore;
//using E_Learning.Services;
//using E_Learning.Areas.Teacher.Controllers;
//namespace E_Learning.Services
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
//            _context.Courses.Add(course);
//            return await _context.SaveChangesAsync() > 0;
//        }

//        public async Task<bool> UpdateAsync(Course course)
//        {
//            _context.Courses.Update(course);
//            return await _context.SaveChangesAsync() > 0;
//        }

//        public async Task<bool> DeleteAsync(int id)
//        {
//            var course = await _context.Courses.FindAsync(id);
//            if (course == null) return false;

//            _context.Courses.Remove(course);
//            return await _context.SaveChangesAsync() > 0;
//        }
//    }
//}
