//using System.Net;
//using System.Net.Mail;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.Extensions.Options;

//namespace E_Learning.Services
//{
//    public class EmailSender : IEmailSender
//    {
//        private readonly EmailSettings _settings;
//        public EmailSender(IOptions<EmailSettings> options)
//        {
//            _settings = options.Value;
//        }

//        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//        {
//            var msg = new MailMessage();
//            msg.From = new MailAddress(_settings.FromEmail, _settings.FromName);
//            msg.To.Add(email);
//            msg.Subject = subject;
//            msg.Body = htmlMessage;
//            msg.IsBodyHtml = true;

//            using var client = new SmtpClient(_settings.Host, _settings.Port)
//            {
//                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
//                EnableSsl = true
//            };

//            await client.SendMailAsync(msg);
//        }
//    }
//}
