using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace E_Learning.Models
{
    public class Student : IEntity
    {
        

        [MaxLength(20)]
        public string? MSSV { get; set; }      // bỏ [Required]



        public int Id { get; set; }

        public  string? UserId { get; set; }
        public User? User { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; } // bỏ [Required]

        [MaxLength(50)]
        public string? LastName { get; set; }  // bỏ [Required]

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
