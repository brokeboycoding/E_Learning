using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using E_Learning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace E_Learning.Areas.Identity.Pages.Account.Manage
{
    public class ResendEmailConfirmationModel(
        UserManager<User> userManager,
        IEmailSender emailSender) : PageModel
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IEmailSender _emailSender = emailSender;

        [BindProperty]
        public required InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string ?Email { get; set; }
        }

        public void OnGet()
        {
            // chỉ hiển thị form
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Input.Email))
            {
                ModelState.AddModelError(string.Empty, "Email không được để trống.");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);

            // nếu không tồn tại hoặc đã confirm rồi, vẫn trả về “thành công” để không lộ thông tin
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["StatusMessage"] =
                    "Nếu email hợp lệ, bạn sẽ nhận được link xác nhận trong hộp thư.";
                return RedirectToPage();
            }

            // tạo token và encode
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
     "/Account/ConfirmEmail",
     pageHandler: null,
     values: new { area = "Identity", userId, code },
     protocol: Request.Scheme);

            if (string.IsNullOrEmpty(callbackUrl))
            {
                TempData["StatusMessage"] = "Không thể tạo liên kết xác nhận.";
                return RedirectToPage();
            }

            await _emailSender.SendEmailAsync(
                Input.Email!,
                "Xác nhận email của bạn",
                $"Vui lòng xác nhận tài khoản của bạn bằng cách " +
                $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click vào đây</a>.");

            return RedirectToPage();
        }
    }
}
