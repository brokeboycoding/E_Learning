using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Hãy nhập mật khẩu mới")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public required string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string? ConfirmPassword { get; set; }
    }

}
