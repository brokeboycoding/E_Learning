using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public Guid? ProfilePictureId { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string FullName => $"{FirstName ?? ""} {LastName ?? ""}".Trim();


        [Display(Name = "Ngày khởi tạo")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Lần đăng nhập cuối")]
        public DateTime? LastLogin { get; set; }
        //public required string Role { get; set; } // Thêm thuộc tính này để lưu vai trò
        //public string? Role { get; set; }     // Vai trò: Admin, Teacher, Student
        public string? MSSV { get; set; } // Thêm MSSV vào tài khoản Student
        public int Points { get; set; } = 0;

    }
}
