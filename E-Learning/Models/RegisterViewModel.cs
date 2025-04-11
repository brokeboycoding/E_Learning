using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "MSSV hoặc Email không được để trống.")]
        [Display(Name = "MSSV (cho Student) hoặc Email (cho Teacher/Admin)")]
        public required string Identifier { get; set; } // Dùng MSSV hoặc Email

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email (Chỉ bắt buộc cho Teacher/Admin)")]
        public required string Email { get; set; } // Email có thể trống nếu là Student

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải từ {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public required string Password { get; set; }


        [Required(ErrorMessage = "Vai trò không được để trống.")]
        [Display(Name = "Vai trò")]
        public required string Role { get; set; } // "Admin", "Teacher" hoặc "Student"
    }

}
