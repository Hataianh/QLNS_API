using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using QLNS_API.Entities;
using QLNS_API.Helpers;
using QLNS_API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        TaiKhoan UpdateTaiKhoan(TaiKhoan taikhoan);
        TaiKhoan AuthenticateByEmail(string email);
    }

    public class UserService : IUserService
    {
        private readonly QLNSContext _db;

        public UserService(QLNSContext dbContext)
        {
            _db = dbContext;
        }

        public User Authenticate(string email, string password)
        {
            var result = from nv in _db.NhanViens
                         join pb in _db.NhanVienPhongBans on nv.MaNhanVien equals pb.MaNhanVien
                         join tpb in _db.PhongBans on pb.MaPhongBan equals tpb.MaPhongBan
                         join bp in _db.NhanVienBoPhans on nv.MaNhanVien equals bp.MaNhanVien
                         join tbp in _db.BoPhans on bp.MaBoPhan equals tbp.MaBoPhan
                         join cv in _db.NhanVienChucVus on nv.MaNhanVien equals cv.MaNhanVien
                         join tcv in _db.ChucVus on cv.MaChucVu equals tcv.MaChucVu
                         join td in _db.TrinhDos on nv.MaTrinhDo equals td.MaTrinhDo
                         join l in _db.NhanVienLuongs on nv.MaNhanVien equals l.MaNhanVien
                         join tk in _db.TaiKhoans on nv.MaNhanVien equals tk.MaNhanVien
                         select new User
                         {
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen,
                             GioiTinh = nv.GioiTinh,
                             NgaySinh = nv.NgaySinh,
                             Cccd = nv.Cccd,
                             DiaChi = nv.DiaChi,
                             HinhAnh = nv.HinhAnh,
                             DienThoai = nv.DienThoai,
                             Email = tk.Email,
                             Password = tk.Password,
                             XacMinhEmail = tk.XacMinhEmail,
                             MaPhongBan = pb.MaPhongBan,
                             NgayKetThucPb = pb.NgayKetThucPb,
                             MaBoPhan = bp.MaBoPhan,
                             NgayKetThucBp = bp.NgayKetThucBp,
                             MaChucVu = cv.MaChucVu,
                             NgayKetThucCv = cv.NgayKetThucCv,
                             MaTrinhDo = nv.MaTrinhDo,
                             TrangThai = nv.TrangThai,
                             TenPhongBan = tpb.TenPhongBan,
                             TenBoPhan = tbp.TenBoPhan,
                             TenChucVu = tcv.TenChucVu,
                             TenTrinhDo = td.TenTrinhDo,
                             MucLuong = l.MucLuong,
                             NgayKetThuc = l.NgayKetThuc,
                             RolePB = tpb.TenPhongBan,
                             RoleBP = tbp.TenBoPhan,
                             RoleCV = tcv.TenChucVu,
                         };
            var user = result.SingleOrDefault(x => x.Email == email && x.Password == password && x.NgayKetThucPb == null && x.NgayKetThucBp == null && x.NgayKetThucCv == null && x.NgayKetThuc == null);

            return user;
        }
        public TaiKhoan AuthenticateByEmail(string email)
        {
            var result = _db.TaiKhoans;
            var taikhoan = result.SingleOrDefault(x => x.Email == email);
            return taikhoan;
        }
        public TaiKhoan UpdateTaiKhoan(TaiKhoan taikhoan)
        {
            try
            {
                var obj_taikhoan = _db.TaiKhoans.SingleOrDefault(x => x.Email == taikhoan.Email);
                if (obj_taikhoan != null)
                {
                    obj_taikhoan.XacMinhEmail = true; // Gán giá trị mới cho trường XacMinhEmail
                    _db.TaiKhoans.Update(obj_taikhoan);
                    _db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    return obj_taikhoan;
                }
                else
                {
                    // Không tìm thấy tài khoản trong cơ sở dữ liệu
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}