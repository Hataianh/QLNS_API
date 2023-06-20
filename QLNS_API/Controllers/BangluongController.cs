using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLNS_API.Entities;
using QLNS_API.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace QLNS_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BangluongController : Controller
    {
        private QLNSContext db = new QLNSContext();

        [Route("loadData")]
        [HttpPost]
        public IActionResult LoadData([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                int? thang = null;
                int? nam = null;
                if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
                if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
                var result = from bl in db.BangLuongs
                             join nv in db.NhanViens on bl.MaNhanVien equals nv.MaNhanVien
                             join pb in db.NhanVienPhongBans on nv.MaNhanVien equals pb.MaNhanVien
                             join tpb in db.PhongBans on pb.MaPhongBan equals tpb.MaPhongBan
                             join bp in db.NhanVienBoPhans on nv.MaNhanVien equals bp.MaNhanVien
                             join tbp in db.BoPhans on bp.MaBoPhan equals tbp.MaBoPhan
                             join cv in db.NhanVienChucVus on nv.MaNhanVien equals cv.MaNhanVien
                             join tcv in db.ChucVus on cv.MaChucVu equals tcv.MaChucVu
                             join td in db.TrinhDos on nv.MaTrinhDo equals td.MaTrinhDo
                             join l in db.NhanVienLuongs on nv.MaNhanVien equals l.MaNhanVien
                             join tk in db.TaiKhoans on nv.MaNhanVien equals tk.MaNhanVien
                             select new
                             {
                                 MaBangLuong = bl.MaBangLuong,
                                 Thang = bl.Thang,
                                 Nam = bl.Nam,
                                 Luong = bl.Luong,
                                 TongNgayCong = bl.TongNgayCong,
                                 TongPhuCap = bl.TongPhuCap,
                                 TongKtkl = bl.TongKtkl,
                                 TongBaoHiem = bl.TongBaoHiem,
                                 TongUngLuong = bl.TongUngLuong,
                                 LuongThucLinh = bl.LuongThucLinh,
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
                                 NgayKetThuc = l.NgayKetThuc
                             };
                var result1 = result.Where(x => x.NgayKetThucPb == null && x.NgayKetThucBp == null && x.NgayKetThucCv == null && x.NgayKetThuc == null && x.Thang == thang && x.Nam == nam).OrderBy(x => x.MaBangLuong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

                return Ok(
                    new Pagination
                    {
                        page = page,
                        totalItem = result1.Count(),
                        pageSize = pageSize,
                        data = result1
                    });

            }
            catch (Exception)
            {
                return Ok("Err");
            }
        }
        [Route("search")]
        [HttpPost]
        public IActionResult Search([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                var hoten = formData.Keys.Contains("hoten") ? (formData["hoten"]).ToString().Trim() : "";
                int? thang = null;
                int? nam = null;
                if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
                if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
                var result = from bl in db.BangLuongs
                             join nv in db.NhanViens on bl.MaNhanVien equals nv.MaNhanVien
                             join pb in db.NhanVienPhongBans on nv.MaNhanVien equals pb.MaNhanVien
                             join tpb in db.PhongBans on pb.MaPhongBan equals tpb.MaPhongBan
                             join bp in db.NhanVienBoPhans on nv.MaNhanVien equals bp.MaNhanVien
                             join tbp in db.BoPhans on bp.MaBoPhan equals tbp.MaBoPhan
                             join cv in db.NhanVienChucVus on nv.MaNhanVien equals cv.MaNhanVien
                             join tcv in db.ChucVus on cv.MaChucVu equals tcv.MaChucVu
                             join td in db.TrinhDos on nv.MaTrinhDo equals td.MaTrinhDo
                             join l in db.NhanVienLuongs on nv.MaNhanVien equals l.MaNhanVien
                             join tk in db.TaiKhoans on nv.MaNhanVien equals tk.MaNhanVien
                             select new
                             {
                                 MaBangLuong = bl.MaBangLuong,
                                 Thang = bl.Thang,
                                 Nam = bl.Nam,
                                 Luong = bl.Luong,
                                 TongNgayCong = bl.TongNgayCong,
                                 TongPhuCap = bl.TongPhuCap,
                                 TongKtkl = bl.TongKtkl,
                                 TongBaoHiem = bl.TongBaoHiem,
                                 TongUngLuong = bl.TongUngLuong,
                                 LuongThucLinh = bl.LuongThucLinh,
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
                                 NgayKetThuc = l.NgayKetThuc
                             };
                var result1 = result.Where(x => x.NgayKetThucPb == null && x.NgayKetThucBp == null && x.NgayKetThucCv == null && x.NgayKetThuc == null && x.HoTen.Contains(hoten)&& x.Thang == thang && x.Nam == nam).OrderBy(x => x.MaBangLuong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

                return Ok(
                    new Pagination
                    {
                        page = page,
                        totalItem = result1.Count(),
                        pageSize = pageSize,
                        data = result1
                    });

            }
            catch (Exception)
            {
                return Ok("Err");
            }
        }
        [Route("get-bangluong")]
        [HttpGet]
        public IActionResult GetBangluong()
        {
            var result = db.BangLuongs;
            return Ok(result);
        }
        [Route("get-by-id/{mabangluong}")]
        [HttpGet]
        public IActionResult GetById(int? mabangluong)
        {
            var result = db.BangLuongs;
            var bangluong = result.SingleOrDefault(x => x.MaBangLuong == mabangluong);
            return Ok(new { bangluong });
        }

        [Route("create-bangluong")]
        [HttpPost]
        public IActionResult CreateBangluong([FromBody] BangLuongModel model)
        {
            // Kiểm tra sự tồn tại của bản ghi
            var existingRecord = db.BangLuongs.FirstOrDefault(x =>
                x.MaNhanVien == model.bangluong.MaNhanVien &&
                x.Thang == model.bangluong.Thang &&
                x.Nam == model.bangluong.Nam);

            if (existingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return Ok(new { data = "Bản ghi đã tồn tại." });
            }

            // Bản ghi chưa tồn tại, tiến hành tạo mới
            db.BangLuongs.Add(model.bangluong);
            db.SaveChanges();

            return Ok(new { data = "Tạo bản ghi thành công." });
        }
        [Route("delete-bangluong/{mabangluong}")]
        [HttpDelete]
        public IActionResult DeleteBangluong(int? mabangluong)
        {
            var obj = db.BangLuongs.SingleOrDefault(s => s.MaBangLuong == mabangluong);
            db.BangLuongs.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
