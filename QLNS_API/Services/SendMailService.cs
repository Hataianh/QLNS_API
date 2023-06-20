using MimeKit;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using static QLNS_API.MailSettings;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Net;

namespace QLNS_API.Services
{
    public class SendMailService
    {
        MailSettings _mailSettings { get; set; }
        public SendMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<string> SendConfirmationEmail(string toEmail, string confirmationLink)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = "Xác minh email";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Vui lòng xác minh email bằng cách <a href='{confirmationLink}'>nhấp vào đây</a>.</p>";
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Lỗi: " + ex.Message;
            }

            smtp.Disconnect(true);
            return "Gửi email thành công";
        }
        public async Task<string> SendResetPasswordEmail(string toEmail, string resetPasswordLink)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = "Đặt lại mật khẩu";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Vui lòng đặt lại mật khẩu bằng cách <a href='{resetPasswordLink}'>nhấp vào đây</a>.</p>";
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Lỗi: " + ex.Message;
            }

            smtp.Disconnect(true);
            return "Gửi email thành công";
        }
    }
    public class MailContent
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

}

