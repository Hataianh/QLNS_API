using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLNS_API.Entities;
using QLNS_API.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;

namespace QLNS_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LuongController : Controller
    {
        private QLNSContext db = new QLNSContext();

        private IQueryable<NhanVienModel> GetNhanViens()
        {
            var result = from nv in db.NhanViens
                         join pb in db.NhanVienPhongBans on nv.MaNhanVien equals pb.MaNhanVien
                         join tpb in db.PhongBans on pb.MaPhongBan equals tpb.MaPhongBan
                         join bp in db.NhanVienBoPhans on nv.MaNhanVien equals bp.MaNhanVien
                         join tbp in db.BoPhans on bp.MaBoPhan equals tbp.MaBoPhan
                         join cv in db.NhanVienChucVus on nv.MaNhanVien equals cv.MaNhanVien
                         join tcv in db.ChucVus on cv.MaChucVu equals tcv.MaChucVu
                         join td in db.TrinhDos on nv.MaTrinhDo equals td.MaTrinhDo
                         join l in db.NhanVienLuongs on nv.MaNhanVien equals l.MaNhanVien
                         join tk in db.TaiKhoans on nv.MaNhanVien equals tk.MaNhanVien
                         select new NhanVienModel
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
                             NgayKetThuc = l.NgayKetThuc
                         };

            return result.Where(x => x.NgayKetThucPb == null && x.NgayKetThucBp == null && x.NgayKetThucCv == null && x.NgayKetThuc == null);
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
                if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
                if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
                var result = from nvl in db.NhanVienLuongs
                             join nv in db.NhanViens on nvl.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 MaLuong = nvl.MaLuong,
                                 MucLuong = nvl.MucLuong,
                                 NgayBatDau = nvl.NgayBatDau,
                                 NgayKetThuc = nvl.NgayKetThuc,
                                 MaNhanVien = nv.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var result1 = result.Where(x => (x.NgayBatDau.HasValue && x.NgayBatDau.Value.Month == thang || thang == null) && (x.NgayBatDau.Value.Year == nam || nam == null)).OrderByDescending(x => x.NgayBatDau).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var result = from nvl in db.NhanVienLuongs
                             join nv in db.NhanViens on nvl.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 MaLuong = nvl.MaLuong,
                                 MucLuong = nvl.MucLuong,
                                 NgayBatDau = nvl.NgayBatDau,
                                 NgayKetThuc = nvl.NgayKetThuc,
                                 MaNhanVien = nv.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var kq = result.Where(x => x.HoTen.Contains(hoten) && (x.NgayBatDau.HasValue && x.NgayBatDau.Value.Month == thang || thang == null) && (x.NgayBatDau.Value.Year == nam || nam == null)).OrderByDescending(x => x.NgayBatDau).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
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
        [Route("get-luong")]
        [HttpGet]
        public IActionResult GetLuong()
        {
            var result = db.NhanVienLuongs;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = from nvl in db.NhanVienLuongs
                         join nv in db.NhanViens on nvl.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             MaLuong = nvl.MaLuong,
                             MucLuong = nvl.MucLuong,
                             NgayBatDau = nvl.NgayBatDau,
                             NgayKetThuc = nvl.NgayKetThuc,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var nhanvienLuong = result.SingleOrDefault(x => x.MaLuong == id);
            return Ok(new { nhanvienLuong });
        }
        [Route("get-by-manhanvien/{manhanvien}")]
        [HttpGet]
        public IActionResult GetByMaNhanVien(int? manhanvien)
        {
            var result = from nvl in db.NhanVienLuongs
                         join nv in db.NhanViens on nvl.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             MaLuong = nvl.MaLuong,
                             MucLuong = nvl.MucLuong,
                             NgayBatDau = nvl.NgayBatDau,
                             NgayKetThuc = nvl.NgayKetThuc,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var nhanvienLuong = result.Where(x => x.MaNhanVien == manhanvien).ToList();
            return Ok(new { nhanvienLuong });
        }

        [Route("create-luong")]
        [HttpPost]
        public IActionResult CreateLuong([FromBody] NhanVienLuongModel model)
        {// Kiểm tra sự tồn tại của bản ghi
            var NVLexistingRecord = db.NhanVienLuongs.FirstOrDefault(x =>
                x.MaNhanVien == model.nhanvienLuong.MaNhanVien &&
                x.MucLuong == model.nhanvienLuong.MucLuong);

            if (NVLexistingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return BadRequest("Bản ghi đã tồn tại." );
            }

            // Bản ghi chưa tồn tại, tiến hành tạo mới
            model.nhanvienLuong.NgayKetThuc = null;
            db.NhanVienLuongs.Add(model.nhanvienLuong);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("update-luong")]
        [HttpPost]
        public IActionResult UpdateLuong([FromBody] NhanVienLuongModel model)
        {
            var obj_nhanvienluong = db.NhanVienLuongs.SingleOrDefault(x => x.MaLuong == model.nhanvienLuong.MaLuong && x.NgayKetThuc == null);

            // Kiểm tra thay đổi về Luong
            if (obj_nhanvienluong != null && obj_nhanvienluong.MucLuong != model.nhanvienLuong.MucLuong)
            {
                // Thay đổi NgayKetThuc của bản ghi cũ thành DateTime.Now
                obj_nhanvienluong.NgayKetThuc = System.DateTime.Now;

                // Tạo bản ghi mới chỉ tại bảng NhanVienLuong
                var newNhanVienLuong = new NhanVienLuong()
                {
                    MaNhanVien = model.nhanvienLuong.MaNhanVien,
                    MucLuong = model.nhanvienLuong.MucLuong,
                    NgayBatDau = System.DateTime.Now,
                    NgayKetThuc = null
                };
                db.NhanVienLuongs.Add(newNhanVienLuong);
            }

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-luong/{id}")]
        [HttpDelete]
        public IActionResult DeleteLuong(int? id)
        {
            var obj = db.NhanVienLuongs.SingleOrDefault(s => s.MaLuong == id);
            db.NhanVienLuongs.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }


        //[Route("get-phucap")]
        //[HttpGet]
        //public IActionResult GetPcById(int? manhanvien)
        //{
        //    var result = from pc in db.PhuCaps
        //                 join nvpc in db.NhanVienPhuCaps on pc.MaPhuCap equals nvpc.MaPhuCap
        //                 join nv in db.NhanViens on nvpc.MaNhanVien equals nv.MaNhanVien
        //                 where nv.MaNhanVien == manhanvien
        //                 select new
        //                 {
        //                     Id = nvpc.Id,
        //                     MaPhuCap = pc.MaPhuCap,
        //                     TenPhuCap = pc.TenPhuCap,
        //                     SoTien = pc.SoTien,
        //                     MaNhanVien = nv.MaNhanVien,
        //                     HoTen = nv.HoTen
        //                 };
        //    var phucap = result.ToList();
        //    var tongphucap = phucap.Sum(x => x.SoTien);
        //    var cpc = phucap.Count();
        //    return Ok(new { phucap, tongphucap, cpc });
        //}

        //[Route("get-baohiem")]
        //[HttpGet]
        //public IActionResult GetBhById(int? manhanvien)
        //{
        //    var result = from bh in db.BaoHiems
        //                 join nvbh in db.NhanVienBaoHiems on bh.MaBaoHiem equals nvbh.MaBaoHiem
        //                 join nv in db.NhanViens on nvbh.MaNhanVien equals nv.MaNhanVien
        //                 where nv.MaNhanVien == manhanvien
        //                 select new
        //                 {
        //                     Id = nvbh.Id,
        //                     MaBaoHiem = bh.MaBaoHiem,
        //                     LoaiBaoHiem = bh.LoaiBaoHiem,
        //                     MucDong = bh.MucDong,
        //                     MaNhanVien = nv.MaNhanVien,
        //                     HoTen = nv.HoTen
        //                 };
        //    var baohiem = result.ToList();
        //    var tongmdbaohiem = baohiem.Sum(x => x.MucDong);
        //    return Ok(new { baohiem, tongmdbaohiem });
        //}

        //[Route("get-ktkl")]
        //[HttpGet]
        //public IActionResult GetKtklById(int? manhanvien, int? thang, int? nam)
        //{
        //    var result = from kt in db.KhenThuongKyLuats
        //                 join nv in db.NhanViens on kt.MaNhanVien equals nv.MaNhanVien
        //                 where nv.MaNhanVien == manhanvien && kt.NgayQuyetDinh.Month == thang && kt.NgayQuyetDinh.Year == nam
        //                 select new
        //                 {
        //                     Id = kt.Id,
        //                     NoiDung = kt.NoiDung,
        //                     NgayQuyetDinh = kt.NgayQuyetDinh,
        //                     Loai = kt.Loai,
        //                     SoTien = kt.SoTien,
        //                     MaNhanVien = kt.MaNhanVien,
        //                     HoTen = nv.HoTen
        //                 };
        //    var khenthuongkyluat = result.ToList();
        //    var khenthuong = khenthuongkyluat.Where(x => x.Loai == 1).Sum(x => x.SoTien);
        //    var kyluat = khenthuongkyluat.Where(x => x.Loai == 0).Sum(x => x.SoTien);
        //    var tongktkl = khenthuong - kyluat;
        //    return Ok(new { khenthuongkyluat, khenthuong, kyluat, tongktkl });
        //}

        //[Route("get-ul")]
        //[HttpGet]
        //public IActionResult GetUlById(int? manhanvien, int? thang, int? nam)
        //{
        //    var result = from ul in db.UngLuongs
        //                 join nv in db.NhanViens on ul.MaNhanVien equals nv.MaNhanVien
        //                 where nv.MaNhanVien == manhanvien && ul.Ngay.Month == thang && ul.Ngay.Year == nam
        //                 select new
        //                 {
        //                     Id = ul.Id,
        //                     Ngay = ul.Ngay,
        //                     NoiDung = ul.NoiDung,
        //                     SoTien = ul.SoTien,
        //                     TrangThai = ul.TrangThai,
        //                     MaNhanVien = ul.MaNhanVien,
        //                     HoTen = nv.HoTen
        //                 };
        //    var ungluong = result.ToList();
        //    var tongungluong = ungluong.Where(x => x.TrangThai == 1).Sum(x => x.SoTien);
        //    return Ok(new { ungluong, tongungluong });
        //}

        //[Route("get-by-id/{manhanvien}")]
        //[HttpGet]
        //public IActionResult GetById(int? manhanvien)
        //{
        //    var result = GetNhanViens();
        //    var nhanvien = result.SingleOrDefault(x => x.MaNhanVien == manhanvien);
        //    return Ok(new { nhanvien });
        //}

        //[HttpGet]
        //[Route("get-all-luong")]
        //public IActionResult GetAllLuong(int? thang, int? nam)
        //{
        //    var result = GetNhanViens(); // Hàm GetNhanViens() trả về danh sách nhân viên
        //    //int? thang = null;
        //    //int? nam = null;

        //    List<object> luongList = new List<object>();
        //    using (var db = new QLNSContext()) // Create a new instance of the DbContext for each query
        //    {
        //        foreach (var nhanvien in result)
        //        {
        //            IActionResult baohiemResult = null;
        //            IActionResult phucapResult = null;
        //            IActionResult ktklResult = null;
        //            IActionResult ungluongResult = null;

        //            using (var scope = new TransactionScope())
        //            {
        //                // GetBhById, GetPcById, GetKtklById, and GetUlById should use the new DbContext instance (db)
        //                baohiemResult = GetBhById( nhanvien.MaNhanVien);
        //                phucapResult = GetPcById( nhanvien.MaNhanVien);
        //                ktklResult = GetKtklById( nhanvien.MaNhanVien, thang, nam);
        //                ungluongResult = GetUlById( nhanvien.MaNhanVien, thang, nam);

        //                scope.Complete();
        //            }

        //            if (baohiemResult is OkObjectResult baohiemObjectResult &&
        //            phucapResult is OkObjectResult phucapObjectResult &&
        //            ktklResult is OkObjectResult ktklObjectResult &&
        //            ungluongResult is OkObjectResult ungluongObjectResult)
        //            {
        //                var baohiemValue = baohiemObjectResult.Value;
        //                var phucapValue = phucapObjectResult.Value;
        //                var ktklValue = ktklObjectResult.Value;
        //                var ungluongValue = ungluongObjectResult.Value;

        //                if (baohiemValue != null && phucapValue != null && ktklValue != null && ungluongValue != null)
        //                {
        //                    var baohiem = baohiemValue.GetType().GetProperty("baohiem")?.GetValue(baohiemValue) as List<object>;
        //                    var phucap = phucapValue.GetType().GetProperty("phucap")?.GetValue(phucapValue) as List<object>;
        //                    var ktkl = ktklValue.GetType().GetProperty("khenthuongkyluat")?.GetValue(ktklValue) as List<object>;
        //                    var ungluong = ungluongValue.GetType().GetProperty("ungluong")?.GetValue(ungluongValue) as List<object>;

        //                    var luong = new
        //                    {
        //                        NhanVien = nhanvien,
        //                        BaoHiem = baohiem,
        //                        PhuCap = phucap,
        //                        KhenThuongKyLuat = ktkl,
        //                        UngLuong = ungluong
        //                    };

        //                    luongList.Add(luong);
        //                }
        //            }
        //        }
        //    }

        //    return Ok(luongList);
        //}
        //[Route("thongtin-luong")]
        //[HttpPost]
        //public IActionResult GetThongtinLuong([FromBody] Dictionary<string, object> formData)
        //{
        //    int? manhanvien = null;
        //    int? thang = null;
        //    int? nam = null;
        //    if (formData.Keys.Contains("manhanvien") && !string.IsNullOrEmpty(Convert.ToString(formData["manhanvien"]))) { manhanvien = int.Parse(formData["manhanvien"].ToString()); }
        //    if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
        //    if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
        //    var nhanvien = GetById(manhanvien);
        //    var baohiem = GetBhById(manhanvien);
        //    var phucap = GetPcById(manhanvien);
        //    var ktkl = GetKtklById(manhanvien, thang, nam);
        //    var ungluong = GetUlById(manhanvien, thang, nam);

        //    return Ok(new { nhanvien, baohiem, phucap, ktkl, ungluong });
        //}
    }
}
