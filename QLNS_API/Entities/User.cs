using System;

namespace QLNS_API.Entities
{
    public class User
    {
        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Cccd { get; set; }
        public string DiaChi { get; set; }
        public string HinhAnh { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? XacMinhEmail { get; set; }
        public string ResetPasswordCode { get; set; }
        public int MaPhongBan { get; set; }
        public DateTime? NgayKetThucPb { get; set; }
        public int MaBoPhan { get; set; }
        public DateTime? NgayKetThucBp { get; set; }
        public int MaChucVu { get; set; }
        public DateTime? NgayKetThucCv { get; set; }
        public int MaTrinhDo { get; set; }
        public int TrangThai { get; set; }
        public string TenPhongBan { get; set; }
        public string TenBoPhan { get; set; }
        public string TenChucVu { get; set; }
        public string TenTrinhDo { get; set; }
        public double MucLuong { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string Token { get; set; }
        public string RolePB { get; set; }
        public string RoleBP { get; set; }
        public string RoleCV { get; set; }
    }
}
