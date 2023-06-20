using Microsoft.AspNetCore.Mvc;
using System;
using QLNS_API.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using QLNS_API.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QLNS_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BangcongController : Controller
    {
        private QLNSContext db = new QLNSContext();

        [HttpPost("cham-cong-vao")]
        public IActionResult ChamCongVao([FromBody] BangCongModel model)
        {
            // Kiểm tra nếu đã chấm công vào trong ngày thì trả về thông báo đã chấm công
            int MaNhanVien = model.MaNhanVien;
            var ngayHienTai = DateTime.Now.Date;
            if (ngayHienTai.DayOfWeek == DayOfWeek.Saturday || ngayHienTai.DayOfWeek == DayOfWeek.Sunday || IsNgayNghi(ngayHienTai))
            {
                return BadRequest("Không thể chấm công vào trong ngày nghỉ.");
            }
            // Kiểm tra nếu đã đăng ký nghỉ trong ngày
            if (IsDaDangKyNghi(MaNhanVien, ngayHienTai))
            {
                return BadRequest("Bạn đã đăng ký nghỉ hôm nay.");
            }
            // Kiểm tra giờ
            var gioVao = DateTime.Now.TimeOfDay;
            var gioQuyDinhVao = new TimeSpan(8, 0, 0);
            string trangThai = gioVao <= gioQuyDinhVao ? null : "Đi muộn";

            // Kiểm tra xem đã chấm công vào trong ngày chưa
            if (IsChamCongTonTai(MaNhanVien, ngayHienTai, "Vào"))
            {
                return BadRequest("Đã chấm công vào trong ngày.");
            }

            // Lưu thông tin chấm công vào CSDL
            BangCong bangCong = new BangCong();
            bangCong.GioVao = DateTime.Now;
            bangCong.MaNhanVien = MaNhanVien;
            bangCong.TrangThaiVao = trangThai;
            bangCong.GioRa = null;
            bangCong.TrangThaiRa = null;
            db.BangCongs.Add(bangCong);
            db.SaveChanges();

            if (trangThai == "Đi muộn")
            {
                // Tạo bản ghi mới trong bảng KhenThuongKyLuat
                KhenThuongKyLuat ktkl = new KhenThuongKyLuat();
                ktkl.NoiDung = "Đi muộn";
                ktkl.NgayQuyetDinh = DateTime.Now;
                ktkl.SoTien = 50000;
                ktkl.Loai = 0;
                ktkl.MaNhanVien = MaNhanVien;
                db.KhenThuongKyLuats.Add(ktkl);
                db.SaveChanges();
            }

            return Ok("Chấm công vào thành công");
        }

        [HttpPost("cham-cong-ra")]
        public IActionResult ChamCongRa([FromBody] BangCongModel model)
        {
            int MaNhanVien = model.MaNhanVien;
            // Kiểm tra nếu đã chấm công ra trong ngày thì trả về thông báo đã chấm công
            var ngayHienTai = DateTime.Now.Date;
            if (ngayHienTai.DayOfWeek == DayOfWeek.Saturday || ngayHienTai.DayOfWeek == DayOfWeek.Sunday || IsNgayNghi(ngayHienTai))
            {
                return BadRequest("Không thể chấm công ra trong ngày nghỉ.");
            }
            // Kiểm tra nếu đã đăng ký nghỉ trong ngày
            if (IsDaDangKyNghi(MaNhanVien, ngayHienTai))
            {
                return BadRequest("Bạn đã đăng ký nghỉ hôm nay.");
            }
            // Kiểm tra giờ
            var gioRa = DateTime.Now.TimeOfDay;
            var gioQuyDinhRa = new TimeSpan(17, 0, 0);
            string trangThai = gioRa >= gioQuyDinhRa ? null : "Về sớm";

            // Kiểm tra xem đã chấm công ra trong ngày chưa
            if (IsChamCongTonTai(MaNhanVien, ngayHienTai, "Ra"))
            {
                return BadRequest("Đã chấm công ra trong ngày.");
            }

            // Lưu thông tin chấm công vào CSDL
            var bangCong = db.BangCongs.SingleOrDefault(x => x.MaNhanVien == MaNhanVien && x.GioVao.HasValue && x.GioVao.Value.Date == ngayHienTai);
            if (bangCong != null)
            {
                bangCong.GioRa = System.DateTime.Now;
                bangCong.TrangThaiRa = trangThai;

                if (bangCong.GioVao == null)
                {
                    bangCong.GioVao = model.GioVao;
                    bangCong.TrangThaiVao = model.TrangThaiVao;
                }

                db.SaveChanges();
            }

            return Ok("Chấm công ra thành công");
        }


        [Route("get-by-nhanvien/{maNhanVien}")]
        [HttpGet]
        public IActionResult GetByNv(int? maNhanVien)
        {
            var ngayHienTai = DateTime.Now.Date;
            var result = from bc in db.BangCongs
                         join nv in db.NhanViens on bc.MaNhanVien equals nv.MaNhanVien
                         select new BangCong
                         {
                             GioVao = bc.GioVao,
                             GioRa = bc.GioRa,
                             MaNhanVien = bc.MaNhanVien,
                             TrangThaiVao = bc.TrangThaiVao,
                         };

            var bangcong = result.FirstOrDefault(x => x.MaNhanVien == maNhanVien && x.GioVao.HasValue && x.GioVao.Value.Date == ngayHienTai);
            if (bangcong == null)
            {
                return NotFound();
            }
            return Ok(new { bangcong });
        }

        private bool IsNgayNghi(DateTime date)
        {
            // Kiểm tra xem ngày có phải là ngày nghỉ không
            var holidays = new List<string> { "01-01", "30-04", "01-05", "02-09", "03-09" }; // Các ngày lễ Việt Nam
            var formattedDate = date.ToString("dd-MM");

            return holidays.Contains(formattedDate);
        }
        private bool IsDaDangKyNghi(int maNhanVien, DateTime ngay)
        {
            return db.NghiPheps.Any(np => np.MaNhanVien == maNhanVien && np.NgayBatDauNghi.Date <= ngay && np.NgayKetThucNghi.Date >= ngay);
        }
        private bool IsChamCongTonTai(int MaNhanVien, DateTime ngay, string type)
        {
            // Kiểm tra xem đã chấm công (Vào hoặc Ra) trong ngày chưa
            return db.BangCongs.Any(x => x.MaNhanVien == MaNhanVien && x.GioVao.HasValue && x.GioVao.Value.Date == ngay && (type.Equals("Vào") ? x.GioVao != null : x.GioRa != null));
        }
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
                int? manhanvien = null;
                int? ngay = null;
                if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
                if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
                if (formData.Keys.Contains("ngay") && !string.IsNullOrEmpty(Convert.ToString(formData["ngay"]))) { ngay = int.Parse(formData["ngay"].ToString()); }
                if (formData.Keys.Contains("manhanvien") && !string.IsNullOrEmpty(Convert.ToString(formData["manhanvien"]))) { manhanvien = int.Parse(formData["manhanvien"].ToString()); }
                    var result = from bc in db.BangCongs
                             join nv in db.NhanViens on bc.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 MaBangCong = bc.MaBangCong,
                                 GioVao = bc.GioVao,
                                 GioRa = bc.GioRa,
                                 TrangThaiVao = bc.TrangThaiVao,
                                 TrangThaiRa = bc.TrangThaiRa,
                                 MaNhanVien = bc.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var result1 = result.Where(x => (x.GioVao.HasValue && x.GioVao.Value.Month == thang || thang == null) && (x.GioVao.Value.Day == ngay || ngay == null) && (x.GioVao.Value.Year == nam || nam == null) && (x.MaNhanVien == manhanvien || manhanvien == null)).OrderByDescending(x => x.MaBangCong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

                return Ok(
                    new Pagination
                    {
                        page = page,
                        totalItem = result.Count(),
                        pageSize = pageSize,
                        data = result1
                    });

            }
            catch (Exception)
            {
                return Ok("Err");
            }
        }
        [Route("bangcongcn")]
        [HttpPost]
        public IActionResult BangCongcn([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                int? thang = null;
                int? nam = null;
                int? manhanvien = null;
                int? ngay = null;
                if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
                if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
                if (formData.Keys.Contains("ngay") && !string.IsNullOrEmpty(Convert.ToString(formData["ngay"]))) { ngay = int.Parse(formData["ngay"].ToString()); }
                if (formData.Keys.Contains("manhanvien") && !string.IsNullOrEmpty(Convert.ToString(formData["manhanvien"]))) { manhanvien = int.Parse(formData["manhanvien"].ToString()); }
                var result = from bc in db.BangCongs
                             join nv in db.NhanViens on bc.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 MaBangCong = bc.MaBangCong,
                                 GioVao = bc.GioVao,
                                 GioRa = bc.GioRa,
                                 TrangThaiVao = bc.TrangThaiVao,
                                 TrangThaiRa = bc.TrangThaiRa,
                                 MaNhanVien = bc.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var result1 = result.Where(x => (x.GioVao.HasValue && x.GioVao.Value.Month == thang || thang == null) && (x.GioVao.Value.Day == ngay || ngay == null) && (x.GioVao.Value.Year == nam || nam == null) && (x.MaNhanVien == manhanvien || manhanvien == null)).OrderByDescending(x => x.MaBangCong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                int? manhanvien = null;
                int? ngay = null;
                if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
                if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
                if (formData.Keys.Contains("ngay") && !string.IsNullOrEmpty(Convert.ToString(formData["ngay"]))) { ngay = int.Parse(formData["ngay"].ToString()); }
                if (formData.Keys.Contains("manhanvien") && !string.IsNullOrEmpty(Convert.ToString(formData["manhanvien"]))) { manhanvien = int.Parse(formData["manhanvien"].ToString()); }
                var result = from bc in db.BangCongs
                             join nv in db.NhanViens on bc.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 MaBangCong = bc.MaBangCong,
                                 GioVao = bc.GioVao,
                                 GioRa = bc.GioRa,
                                 TrangThaiVao = bc.TrangThaiVao,
                                 TrangThaiRa = bc.TrangThaiRa,
                                 MaNhanVien = bc.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var kq = result.Where(x => x.HoTen.Contains(hoten) && (x.GioVao.HasValue && x.GioVao.Value.Month == thang || thang == null) && (x.GioVao.Value.Day == ngay || ngay == null) && (x.GioVao.Value.Year == nam || nam == null) && (x.MaNhanVien == manhanvien || manhanvien == null)).OrderByDescending(x => x.MaBangCong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                return Ok(new Pagination
                {
                    page = page,
                    totalItem = result.Count(),
                    pageSize = pageSize,
                    data = kq
                });

            }
            catch (Exception)
            {
                return Ok("Err");
            }
        }
    }
}
