using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Models
{
    public class Student : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Mã số sinh viên")]
        [Required(ErrorMessage = "Hãy nhập MSSV")]
        [MaxLength(20)]
        public string MSSV { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Dùng IdentityUser nếu không có ApplicationUser
        public IdentityUser User { get; set; } = null!;

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;


        [Display(Name = "Ngày bắt đầu học")]
        public DateTime? EnrollmentDate { get; set; }

        public string? FormattedEnrollmentDate => EnrollmentDate?.ToString("dd/MM/yyyy");

        [Display(Name = "Tình trạng")]
        public StudentStatus Status { get; set; } = StudentStatus.Pending;

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }

    public enum StudentStatus
    {
        Pending,
        Active,
        Inactive
    }
}
