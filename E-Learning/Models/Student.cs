using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace E_Learning.Models
{
    public class Student : IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Ngày bắt đầu học")]
        public DateTime? EnrollmentDate { get; set; }

        public string FormattedEnrollmentDate => EnrollmentDate?.ToString("dd/MM/yyyy");


        [Display(Name = "Tình trạng")]
        public StudentStatus Status { get; set; } = StudentStatus.Pending;

        public int? SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();


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
