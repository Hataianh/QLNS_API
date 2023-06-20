using QLNS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApi.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QLNS_API.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using QLNS_API.Services;
using QLNS_API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace QLNS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly SendMailService _sendMailService;
        private readonly AppSettings _appSettings;

        public UserController(IOptions<AppSettings> appSettings, IUserService userService, SendMailService sendMailService, IConfiguration configuration, QLNSContext dbContext)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _sendMailService = sendMailService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);
            // return null if user not found
            if (user == null)
                return BadRequest( "Tài khoản hoặc mật khẩu sai!" );
            //var passwordHasher = new PasswordHasher();
            //var passwordVerificationResult = PasswordHasher.VerifyHashedPassword(null, user.Password, model.Password);
            if (!VerifyPassword(model.Password, user.Password))
            {
                return Unauthorized();
            }

            if ((bool)!user.XacMinhEmail)
            {
                await SendEmailVerification(user);
            }

            var token = GenerateJwtToken(user);
            user.Token = token;
            return Ok( user.WithoutPassword() );
        }
        
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.HoTen.ToString()),
                    new Claim(ClaimTypes.MobilePhone, user.DienThoai.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }

        // authentication successful so generate jwt token
       
        [HttpPost("send-email-verification")]
        public async Task SendEmailVerification(User user)
        {
            // Tạo confirmation link dựa trên đường dẫn frontend của bạn
            var confirmationLink = $"https://saikosofware.web.app/xacminh-email?email={user.Email}";

            await _sendMailService.SendConfirmationEmail(user.Email, confirmationLink);
            // Xử lý kết quả gửi email (result) nếu cần
        }
       
        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public IActionResult ConfirmEmail([FromBody] ConfirmEmail model)
        {
            var taikhoan = _userService.AuthenticateByEmail(model.Email);
            if (taikhoan == null)
                return BadRequest( "Không tìm thấy người dùng" );

            if (taikhoan.XacMinhEmail)
                return BadRequest( "Email đã được xác minh trước đó" );

            // Cập nhật trạng thái xác minh email của người dùng
            taikhoan.XacMinhEmail = true;
            _userService.UpdateTaiKhoan(taikhoan);

            return Ok(new { message = "Xác minh email thành công" });
        }
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var taikhoan = _userService.AuthenticateByEmail(model.Email);

            // Kiểm tra xem tài khoản có tồn tại hay không
            if (taikhoan == null)
                return BadRequest("Không tìm thấy người dùng với địa chỉ email này!" );

            // Tạo mã xác nhận đặt lại mật khẩu
            var ResetPasswordCode = GeneratePasswordResetCode();

            // Lưu mã xác nhận vào cơ sở dữ liệu
            taikhoan.ResetPasswordCode = ResetPasswordCode;
            _userService.UpdateTaiKhoan(taikhoan);

            // Gửi email đặt lại mật khẩu
            await SendPasswordResetEmail(taikhoan.Email, ResetPasswordCode);

            return Ok(new { message = "Email đặt lại mật khẩu đã được gửi!" });
        }
        private string GeneratePasswordResetCode()
        {
            // Tạo một chuỗi ngẫu nhiên làm mã xác nhận
            var ResetPasswordCode = Guid.NewGuid().ToString();

            return ResetPasswordCode;
        }
        private async Task SendPasswordResetEmail(string email, string ResetPasswordCode)
        {
            // Tạo liên kết đặt lại mật khẩu với mã xác nhận
            var passwordResetLink = $"https://saikosofware.web.app/reset-password?email={email}&ResetPasswordCode={ResetPasswordCode}";

            // Gửi email
            await _sendMailService.SendResetPasswordEmail(email, passwordResetLink);
        }
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            var taikhoan = _userService.AuthenticateByEmail(model.Email);

            // Kiểm tra xem tài khoản có tồn tại hay không
            if (taikhoan == null)
                return BadRequest( "Không tìm thấy người dùng với địa chỉ email này!" );

            // Kiểm tra xem mã xác nhận đặt lại mật khẩu có hợp lệ hay không
            if (taikhoan.ResetPasswordCode != model.ResetPasswordCode)
                return BadRequest( "Mã xác nhận đặt lại mật khẩu không hợp lệ!" );

            // Cập nhật mật khẩu mới
            taikhoan.Password = model.NewPassword;
            taikhoan.ResetPasswordCode = null; // Xóa mã xác nhận đặt lại mật khẩu sau khi đã sử dụng

            _userService.UpdateTaiKhoan(taikhoan);

            return Ok(new { message = "Mật khẩu đã được đặt lại thành công!" });
        }
    }
    public class ConfirmEmail
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string ResetPasswordCode { get; set; }
        public string NewPassword { get; set; }
    }
}