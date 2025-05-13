using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics;
using E_Learning.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public required DbSet<User> User { get; set; }
        public required DbSet<Course> Courses { get; set; }
        public required DbSet<Student> Students { get; set; }
    
       
        public   required DbSet<Teacher> Teachers { get; set; }
        
       
        public required DbSet<Grade> Grades { get; set; }
      
        public required DbSet<Alert> Alerts { get; set; }
      
        public required DbSet<Certificate> Certificates { get; set; }
        public required DbSet <ExamSubmission> ExamSubmissions { get; set; }
        public required DbSet <Resource> Resources {  get; set; }

        public required DbSet<Module> Modules { get; set; }

        public required DbSet<Lesson> Lessons { get; set; }
        public required DbSet<LessonProgress> lessonProgresses { get; set; }
        public required DbSet<Discussion> Discussions { get; set; }
        public required DbSet<Enrollment> Enrollments { get; set; }
        public required DbSet<LessonNote> LessonNotes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
    .HasIndex(e => new { e.UserId, e.CourseId })
    .IsUnique(); // 1 sinh viên chỉ đăng ký 1 lần cho 1 khóa




            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);


           



         


           
        }
    }
}
