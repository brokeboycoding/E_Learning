using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace E_Learning.Models;

public partial class ELearning2Context : DbContext
{
    public ELearning2Context()
    {
    }

    public ELearning2Context(DbContextOptions<ELearning2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetroleclaim> Aspnetroleclaims { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserclaim> Aspnetuserclaims { get; set; }

    public virtual DbSet<Aspnetuserlogin> Aspnetuserlogins { get; set; }

    public virtual DbSet<Aspnetusertoken> Aspnetusertokens { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Discussion> Discussions { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Examsubmission> Examsubmissions { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Lessonnote> Lessonnotes { get; set; }

    public virtual DbSet<Lessonprogress> Lessonprogresses { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=e_learning2;user=root;password=p882004N@@", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alerts");

            entity.Property(e => e.CreatedAt).HasMaxLength(6);
            entity.Property(e => e.Message).HasMaxLength(500);
        });

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetroles");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<Aspnetroleclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetroleclaims");

            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Aspnetroleclaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetusers");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.DateCreated).HasMaxLength(6);
            entity.Property(e => e.Discriminator).HasMaxLength(13);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastLogin).HasMaxLength(6);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LockoutEnd).HasMaxLength(6);
            entity.Property(e => e.Mssv).HasColumnName("MSSV");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.ProfilePictureId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Aspnetuserrole",
                    r => r.HasOne<Aspnetrole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId"),
                    l => l.HasOne<Aspnetuser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("aspnetuserroles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Aspnetuserclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetuserclaims");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserclaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetuserlogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("aspnetuserlogins");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserlogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetusertoken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("aspnetusertokens");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetusertokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("certificates");

            entity.HasIndex(e => e.CourseId, "IX_Certificates_CourseId");

            entity.HasIndex(e => e.GradeId, "IX_Certificates_GradeId");

            entity.HasIndex(e => e.StudentId, "IX_Certificates_StudentId");

            entity.Property(e => e.IssueDate).HasMaxLength(6);

            entity.HasOne(d => d.Course).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Certificates_Courses_CourseId");

            entity.HasOne(d => d.Grade).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.GradeId)
                .HasConstraintName("FK_Certificates_Grades_GradeId");

            entity.HasOne(d => d.Student).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Certificates_AspNetUsers_StudentId");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("courses");

            entity.Property(e => e.CreatedAt).HasMaxLength(6);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasMaxLength(6);
        });

        modelBuilder.Entity<Discussion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("discussions");

            entity.HasIndex(e => e.LessonId, "IX_Discussions_LessonId");

            entity.HasIndex(e => e.UserId, "IX_Discussions_UserId");

            entity.Property(e => e.CreatedAt).HasMaxLength(6);

            entity.HasOne(d => d.Lesson).WithMany(p => p.Discussions)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_Discussions_Lessons_LessonId");

            entity.HasOne(d => d.User).WithMany(p => p.Discussions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Discussions_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("enrollments");

            entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "IX_Enrollments_UserId_CourseId").IsUnique();

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Enrollments_Courses_CourseId");

            entity.HasOne(d => d.User).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Enrollments_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Examsubmission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("examsubmissions");

            entity.HasIndex(e => e.CourseId, "IX_ExamSubmissions_CourseId");

            entity.HasIndex(e => e.StudentId, "IX_ExamSubmissions_StudentId");

            entity.Property(e => e.SubmissionDate).HasMaxLength(6);

            entity.HasOne(d => d.Course).WithMany(p => p.Examsubmissions)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_ExamSubmissions_Courses_CourseId");

            entity.HasOne(d => d.Student).WithMany(p => p.Examsubmissions)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_ExamSubmissions_AspNetUsers_StudentId");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("grades");

            entity.HasIndex(e => e.StudentId, "IX_Grades_StudentId");

            entity.Property(e => e.EvaluationDate).HasMaxLength(6);

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Grades_Students_StudentId");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lessons");

            entity.HasIndex(e => e.ModuleId, "IX_Lessons_ModuleId");

            entity.HasOne(d => d.Module).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK_Lessons_Modules_ModuleId");
        });

        modelBuilder.Entity<Lessonnote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lessonnotes");

            entity.HasIndex(e => e.LessonId, "IX_LessonNotes_LessonId");

            entity.HasIndex(e => e.UserId, "IX_LessonNotes_UserId");

            entity.Property(e => e.Timestamp).HasMaxLength(6);

            entity.HasOne(d => d.Lesson).WithMany(p => p.Lessonnotes)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_LessonNotes_Lessons_LessonId");

            entity.HasOne(d => d.User).WithMany(p => p.Lessonnotes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_LessonNotes_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Lessonprogress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lessonprogresses");

            entity.HasIndex(e => e.LessonId, "IX_lessonProgresses_LessonId");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Lessonprogresses)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_lessonProgresses_Lessons_LessonId");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("modules");

            entity.HasIndex(e => e.CourseId, "IX_Modules_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.Modules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Modules_Courses_CourseId");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("resources");

            entity.HasIndex(e => e.CourseId, "IX_Resources_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.Resources)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Resources_Courses_CourseId");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("students");

            entity.HasIndex(e => e.UserId, "IX_Students_UserId");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasComment("Khi mà học sinh vô hồ sơ thì sẽ điền vô");
            entity.Property(e => e.EnrollmentDate).HasMaxLength(6);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasDefaultValueSql("'0'");
            entity.Property(e => e.ImageId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Mssv)
                .HasMaxLength(20)
                .HasColumnName("MSSV");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Students_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("teachers");

            entity.HasIndex(e => e.UserId, "IX_Teachers_UserId");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasComment("Bắt buộc phải có");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.HireDate).HasMaxLength(6);
            entity.Property(e => e.ImageId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Teachers_AspNetUsers_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
