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
        public required DbSet<Subject> Subjects { get; set; }
        public required DbSet<SchoolClass> SchoolClasses { get; set; }
        public   required DbSet<Teacher> Teachers { get; set; }
        
        public required DbSet<Attendance> Attendances { get; set; }
        public required DbSet<Grade> Grades { get; set; }
        public required DbSet<Payment> Payments { get; set; }
        public required DbSet<Alert> Alerts { get; set; }
        public required DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public required DbSet<TeacherSchoolClass> TeacherSchoolClasses { get; set; }
        public required DbSet<CourseSubject> CourseSubjects { get; set; }
        public required DbSet<Certificate> Certificates { get; set; }
        public required DbSet <ExamSubmission> ExamSubmissions { get; set; }
        public required DbSet <Resource> Resources {  get; set; }


    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Student>()
                .HasOne(s => s.SchoolClass)
                .WithMany(sc => sc.Students)
                .HasForeignKey(s => s.SchoolClassId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TeacherSubject>()
                .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<CourseSubject>()
                .HasKey(cs => new { cs.CourseId, cs.SubjectId });

            modelBuilder.Entity<CourseSubject>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseSubjects)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseSubject>()
                .HasOne(cs => cs.Subject)
                .WithMany(s => s.CourseSubjects)
                .HasForeignKey(cs => cs.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchoolClass>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.SchoolClasses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.SetNull);



            modelBuilder.Entity<TeacherSchoolClass>()
                .HasKey(tsc => new { tsc.TeacherId, tsc.SchoolClassId }); 

            modelBuilder.Entity<TeacherSchoolClass>()
                .HasOne(tsc => tsc.Teacher)
                .WithMany(t => t.TeacherSchoolClasses)
                .HasForeignKey(tsc => tsc.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeacherSchoolClass>()
                .HasOne(tsc => tsc.SchoolClass)
                .WithMany(sc => sc.TeacherSchoolClasses)
                .HasForeignKey(tsc => tsc.SchoolClassId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
