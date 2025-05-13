using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Teacher : IEntity
    {
        
        public int Id { get; set; }

        public string? UserId { get; set; }

      
        public User? User { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Hãy nhập vào")]
        [MaxLength(50)]
        public string? LastName { get; set; }

      
     

        
        [Display(Name = "Ngày bắt đầu dạy")]
        public DateTime? HireDate { get; set; }

       
        public string? FormattedHireDate => HireDate?.ToString("dd/MM/yyyy");

        
        [Display(Name = "Tình trạng")]
        public TeacherStatus Status { get; set; } = TeacherStatus.Active;

       

        
        [Display(Name = "Hình đại diện")]
        public Guid ImageId { get; set; }

        
      
    }
    public enum TeacherStatus
    {
        Pending,
        Active,
        Inactive
    }
}
