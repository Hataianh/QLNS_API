using System;
using System.Collections.Generic;

#nullable disable

namespace QLNS_API.Models
{
    public partial class TaiKhoan
    {
        public int MaTaiKhoan { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool XacMinhEmail { get; set; }
        public string ResetPasswordCode { get; set; }
        public int MaNhanVien { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
