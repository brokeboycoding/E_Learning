using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace E_Learning.Models
{
    public class Student : IEntity
    {
        [Display(Name = "Mã số sinh viên")]
        [Required(ErrorMessage = "Hãy nhập MSSV")]
        [MaxLength(20)]
        public string MSSV { get; set; } = string.Empty;


        public int Id { get; set; }

        public required string UserId { get; set; }
        public required User User { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Display(Name = "Ngày bắt đầu học")]
        public DateTime? EnrollmentDate { get; set; }

        public string? FormattedEnrollmentDate => EnrollmentDate?.ToString("dd/MM/yyyy");


        [Display(Name = "Tình trạng")]
        public StudentStatus Status { get; set; } = StudentStatus.Pending;

   
       
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();

      


        [Display(Name = "Image")]
        public Guid ImageId { get; set; }
    }
    public enum StudentStatus
    {
        Pending,
        Active,
        Inactive
    }
}
