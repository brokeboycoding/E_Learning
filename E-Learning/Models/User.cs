using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public Guid? ProfilePictureId { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Ngày khởi tạo")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Lần đăng nhập cuối")]
        public DateTime? LastLogin { get; set; }
    }
}
