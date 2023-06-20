using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using QLNS_API.Models;
using System.Linq;
using QLNS_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace QLNS_API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NhanvienController : Controller
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
        
        [Route("bo-loc")]
        [HttpPost]
        public IActionResult Boloc([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                int? phongban = null;
                int? trinhdo = null;
                int? bophan = null;
                int? chucvu = null;
                int? loaitrangthai = null;
                if (formData.Keys.Contains("phongban") && !string.IsNullOrEmpty(Convert.ToString(formData["phongban"]))) { phongban = int.Parse(formData["phongban"].ToString()); }
                if (formData.Keys.Contains("trinhdo") && !string.IsNullOrEmpty(Convert.ToString(formData["trinhdo"]))) { trinhdo = int.Parse(formData["trinhdo"].ToString()); }
                if (formData.Keys.Contains("bophan") && !string.IsNullOrEmpty(Convert.ToString(formData["bophan"]))) { bophan = int.Parse(formData["bophan"].ToString()); }
                if (formData.Keys.Contains("chucvu") && !string.IsNullOrEmpty(Convert.ToString(formData["chucvu"]))) { chucvu = int.Parse(formData["chucvu"].ToString()); }
                if (formData.Keys.Contains("loaitrangthai") && !string.IsNullOrEmpty(Convert.ToString(formData["loaitrangthai"]))) { loaitrangthai = int.Parse(formData["loaitrangthai"].ToString()); }


                var result = GetNhanViens();
                var result1 = result.Where(x => (x.MaPhongBan == phongban || phongban == null)
                                            && (x.MaTrinhDo == trinhdo || trinhdo == null)
                                            && (x.MaBoPhan == bophan || bophan == null)
                                            && (x.MaChucVu == chucvu || chucvu == null)
                                            && (x.TrangThai == loaitrangthai || loaitrangthai == null))
                                    .OrderBy(x => x.MaNhanVien)
                                    .Skip(pageSize * (page - 1))
                                    .Take(pageSize)
                                    .ToList();
                return Ok(
                           new Pagination
                           {
                               page = page,
                               totalItem = result.Count(),
                               pageSize = pageSize,
                               data = result1
                           }
                         );

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

                var result = GetNhanViens();
                var result1 = result.Where(x => x.HoTen.Contains(hoten)).OrderBy(x => x.MaNhanVien).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

                return Ok(
                           new Pagination
                           {
                               page = page,
                               totalItem = result.Count(),
                               pageSize = pageSize,
                               data = result1
                           }
                         );

            }
            catch (Exception)
            {
                return Ok("Err");
            }
        }
        [Route("get-LSphongban")]
        [HttpGet]
        public IActionResult GetLsPbById(int? manhanvien)
        {
            var result = from pb in db.PhongBans
                         join nvpb in db.NhanVienPhongBans on pb.MaPhongBan equals nvpb.MaPhongBan
                         join nv in db.NhanViens on nvpb.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = nvpb.Id,
                             MaPhongBan = pb.MaPhongBan,
                             TenPhongBan = pb.TenPhongBan,
                             NgayBatDauPb = nvpb.NgayBatDauPb,
                             NgayKetThucPb = nvpb.NgayKetThucPb,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var phongban = result.Where(x => x.MaNhanVien == manhanvien).ToList();
            
            return Ok(new { phongban });
        }
        [Route("get-LSbophan")]
        [HttpGet]
        public IActionResult GetLsBpById(int? manhanvien)
        {
            var result = from bp in db.BoPhans
                         join nvbp in db.NhanVienBoPhans on bp.MaBoPhan equals nvbp.MaBoPhan
                         join nv in db.NhanViens on nvbp.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = nvbp.Id,
                             MaBoPhan = bp.MaBoPhan,
                             TenBoPhan = bp.TenBoPhan,
                             NgayBatDauBp = nvbp.NgayBatDauBp,
                             NgayKetThucBp = nvbp.NgayKetThucBp,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var bophan = result.Where(x => x.MaNhanVien == manhanvien).ToList();

            return Ok(new { bophan });
        }
        [Route("get-LSchucvu")]
        [HttpGet]
        public IActionResult GetLsCvById(int? manhanvien)
        {
            var result = from cv in db.ChucVus
                         join nvcv in db.NhanVienChucVus on cv.MaChucVu equals nvcv.MaChucVu
                         join nv in db.NhanViens on nvcv.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = nvcv.Id,
                             MaChucVu = cv.MaChucVu,
                             TenChucVu = cv.TenChucVu,
                             NgayBatDauCv = nvcv.NgayBatDauCv,
                             NgayKetThucCv = nvcv.NgayKetThucCv,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var chucvu = result.Where(x => x.MaNhanVien == manhanvien).ToList();

            return Ok(new { chucvu });
        }
        [Route("get-LScongtac/{manhanvien}")]
        [HttpGet]
        public IActionResult GetNvById(int? manhanvien)
        {
            var result = from nv in db.NhanViens
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
                             MaTrinhDo = nv.MaTrinhDo,
                             TrangThai = nv.TrangThai,
                             MucLuong = l.MucLuong,
                             NgayKetThuc = l.NgayKetThuc
                         };
            var nhanvien = result.Where(x => x.MaNhanVien == manhanvien && x.NgayKetThuc == null).ToList();
            var phongban = GetLsPbById(manhanvien);
            var bophan = GetLsBpById(manhanvien);
            var chucvu = GetLsCvById(manhanvien);
            return Ok(new { nhanvien, phongban, bophan, chucvu });
        }
        [Route("get-phucap")]
        [HttpGet]
        public IActionResult GetPcById(int? manhanvien)
        {
            var result = from pc in db.PhuCaps
                         join nvpc in db.NhanVienPhuCaps on pc.MaPhuCap equals nvpc.MaPhuCap
                         join nv in db.NhanViens on nvpc.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = nvpc.Id,
                             MaPhuCap = pc.MaPhuCap,
                             TenPhuCap = pc.TenPhuCap,
                             SoTien = pc.SoTien,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var phucap = result.Where(x => x.MaNhanVien == manhanvien).ToList();
            var tongphucap = phucap.Sum(x => x.SoTien);
            var cpc = phucap.Count();
            return Ok(new { phucap, tongphucap , cpc});
        }
        [Route("get-baohiem")]
        [HttpGet]
        public IActionResult GetBhById(int? manhanvien)
        {
            var result = from bh in db.BaoHiems
                         join nvbh in db.NhanVienBaoHiems on bh.MaBaoHiem equals nvbh.MaBaoHiem
                         join nv in db.NhanViens on nvbh.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = nvbh.Id,
                             MaBaoHiem = bh.MaBaoHiem,
                             LoaiBaoHiem = bh.LoaiBaoHiem,
                             MucDong = bh.MucDong,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var baohiem = result.Where(x => x.MaNhanVien == manhanvien).ToList();
            var tongmdbaohiem = baohiem.Sum(x => x.MucDong);
            return Ok(new { baohiem, tongmdbaohiem });
        }
        
        [Route("get-ktkl")]
        [HttpGet]
        public IActionResult GetKtklById(int? manhanvien, int? thang, int? nam)
        {
            var result = from kt in db.KhenThuongKyLuats
                         join nv in db.NhanViens on kt.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = kt.Id,
                             NoiDung = kt.NoiDung,
                             NgayQuyetDinh = kt.NgayQuyetDinh,
                             Loai = kt.Loai,
                             SoTien = kt.SoTien,
                             MaNhanVien = kt.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var khenthuongkyluat = result.Where(x => x.MaNhanVien == manhanvien && x.NgayQuyetDinh.Month == thang && x.NgayQuyetDinh.Year == nam).ToList();
            var khenthuong = khenthuongkyluat.Where(x => x.Loai == 1).Sum(x => x.SoTien);
            var kyluat = khenthuongkyluat.Where(x => x.Loai == 0).Sum(x => x.SoTien);
            var tongktkl = khenthuong - kyluat;
            return Ok(new { khenthuongkyluat, khenthuong, kyluat, tongktkl });
        }
        [Route("get-ul")]
        [HttpGet]
        public IActionResult GetUlById(int? manhanvien, int? thang, int? nam)
        {
            var result = from ul in db.UngLuongs
                         join nv in db.NhanViens on ul.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = ul.Id,
                             Ngay = ul.Ngay,
                             NoiDung = ul.NoiDung,
                             SoTien = ul.SoTien,
                             TrangThai = ul.TrangThai,
                             MaNhanVien = ul.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var ungluong = result.Where(x => x.MaNhanVien == manhanvien && x.Ngay.Month == thang && x.Ngay.Year == nam).ToList();
            var tongungluong = ungluong.Where(x => x.TrangThai == 1).Sum(x => x.SoTien);
            return Ok(new { ungluong, tongungluong });
        }
        [Route("get-bc")]
        [HttpGet]
        public IActionResult GetBcById(int? manhanvien, int? thang, int? nam)
        {
            var result = from bc in db.BangCongs
                         join nv in db.NhanViens on bc.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             MaBangCong = bc.MaBangCong,
                             GioVao = bc.GioVao,
                             GioRa = bc.GioRa,
                             TrangThaiVao = bc.TrangThaiVao,
                             TrangThaiRa = bc.TrangThaiRa,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var bangcong = result.Where(x => x.MaNhanVien == manhanvien && x.GioVao.HasValue && x.GioVao.Value.Month == thang && x.GioVao.Value.Year == nam && x.GioRa.HasValue && x.GioRa.Value.Month == thang && x.GioRa.Value.Year == nam).ToList();
            var tongngaycong = bangcong.Count;
            return Ok(new { bangcong, tongngaycong });
        }
        [Route("get-by-id/{manhanvien}")]
        [HttpGet]
        public IActionResult GetById(int? manhanvien)
        {
            var result = GetNhanViens();
            var nhanvien = result.SingleOrDefault(x => x.MaNhanVien == manhanvien);
            return Ok(new { nhanvien });
        }
        //[Route("get-all-luong")]
        //[HttpGet]
        //public IActionResult GetAllLuong()
        //{
        //    var result = GetNhanViens(); // Hàm GetNhanViens() trả về danh sách nhân viên
        //    int? thang = null;
        //    int? nam = null;

        //    List<object> luongList = new List<object>();
        //    foreach (var nhanvien in result)
        //    {
        //        var baohiemResult = GetBhById(nhanvien.MaNhanVien);
        //        var phucapResult = GetPcById(nhanvien.MaNhanVien);
        //        var ktklResult = GetKtklById(nhanvien.MaNhanVien, thang, nam);
        //        var ungluongResult = GetUlById(nhanvien.MaNhanVien, thang, nam);

        //        if (baohiemResult is OkObjectResult baohiemObjectResult &&
        //            phucapResult is OkObjectResult phucapObjectResult &&
        //            ktklResult is OkObjectResult ktklObjectResult &&
        //            ungluongResult is OkObjectResult ungluongObjectResult)
        //        {
        //            var baohiem = baohiemObjectResult.Value;
        //            var phucap = phucapObjectResult.Value;
        //            var ktkl = ktklObjectResult.Value;
        //            var ungluong = ungluongObjectResult.Value;

        //            var luong = new
        //            {
        //                NhanVien = nhanvien,
        //                BaoHiem = baohiem,
        //                PhuCap = phucap,
        //                KhenThuongKyLuat = ktkl,
        //                UngLuong = ungluong
        //            };

        //            luongList.Add(luong);
        //        }
        //    }

        //    return Ok(luongList);
        //}
        [Route("thongtin-luong")]
        [HttpPost]
        public IActionResult GetThongtinLuong([FromBody] Dictionary<string, object> formData)
        {
            int? manhanvien = null;
            int? thang = null;
            int? nam = null;
            if (formData.Keys.Contains("manhanvien") && !string.IsNullOrEmpty(Convert.ToString(formData["manhanvien"]))) { manhanvien = int.Parse(formData["manhanvien"].ToString()); }
            if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
            if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
            var nhanvien = GetById(manhanvien);
            var baohiem = GetBhById(manhanvien);
            var phucap = GetPcById(manhanvien);
            var ktkl = GetKtklById(manhanvien, thang, nam);
            var ungluong = GetUlById(manhanvien, thang, nam);
            var bangcong = GetBcById(manhanvien, thang, nam);

            return Ok(new { nhanvien, baohiem, phucap, ktkl, ungluong, bangcong });
        }
        

        [Route("get-nhanvien")]
        [HttpGet]
        public IActionResult GetNhanvien()
        {
            var result = GetNhanViens();

            return Ok(result.OrderBy(x => x.MaNhanVien).ToList());
        }
        
        

        [Route("create-nhanvien")]
        [HttpPost]
        public IActionResult CreateNhanvien([FromBody] NhanVienModel model)
        {
            // Kiểm tra sự tồn tại của bản ghi
            var NVexistingRecord = db.NhanViens.FirstOrDefault(x =>
                x.HoTen == model.nhanvien.HoTen &&
                x.GioiTinh == model.nhanvien.GioiTinh &&
                x.DiaChi == model.nhanvien.DiaChi &&
                x.Cccd == model.nhanvien.Cccd &&
                x.DienThoai == model.nhanvien.DienThoai &&
               x.NgaySinh == model.nhanvien.NgaySinh);

            if (NVexistingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return BadRequest("Bản ghi đã tồn tại." );
            }
            model.nhanvien.TrangThai = 1;
            db.NhanViens.Add(model.nhanvien);
            db.SaveChanges();


            int MaNhanVien = model.nhanvien.MaNhanVien;
            // Kiểm tra sự tồn tại của bản ghi
            var TKexistingRecord = db.TaiKhoans.FirstOrDefault(x =>
                x.Email == model.taikhoan.Email);

            if (TKexistingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return BadRequest("Tài khoản đã tồn tại." );
            }
            TaiKhoan tk = new TaiKhoan();
            //var passwordHasher = new PasswordHasher<TaiKhoan>();
            //string hashedPassword = passwordHasher.HashPassword(null, model.taikhoan.Password);
            tk.MaNhanVien = MaNhanVien;
            tk.Email = model.taikhoan.Email;
            tk.Password = model.taikhoan.Password;
            tk.XacMinhEmail = false;
            db.TaiKhoans.Add(tk);
            db.SaveChanges();
            // Kiểm tra sự tồn tại của bản ghi
            var PBexistingRecord = db.NhanVienPhongBans.FirstOrDefault(x =>
                x.MaNhanVien == model.nhanvienPhongban.MaNhanVien &&
                x.MaPhongBan == model.nhanvienPhongban.MaPhongBan);

            if (PBexistingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return Ok(new { data = "Bản ghi đã tồn tại." });
            }

            // Bản ghi chưa tồn tại, tiến hành tạo mới
            NhanVienPhongBan nvpb = new NhanVienPhongBan();
            nvpb.MaNhanVien = MaNhanVien;
            nvpb.MaPhongBan = model.nhanvienPhongban.MaPhongBan;
            nvpb.NgayBatDauPb = System.DateTime.Now;
            nvpb.NgayKetThucPb = null;
            db.NhanVienPhongBans.Add(nvpb);
            db.SaveChanges();
            // Kiểm tra sự tồn tại của bản ghi
            var BPexistingRecord = db.NhanVienBoPhans.FirstOrDefault(x =>
                x.MaNhanVien == model.nhanvienBophan.MaNhanVien &&
                x.MaBoPhan == model.nhanvienBophan.MaBoPhan);

            if (BPexistingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return Ok(new { data = "Bản ghi đã tồn tại." });
            }

            // Bản ghi chưa tồn tại, tiến hành tạo mới
            NhanVienBoPhan nvbp = new NhanVienBoPhan();
            nvbp.MaNhanVien = MaNhanVien;
            nvbp.MaBoPhan = model.nhanvienBophan.MaBoPhan;
            nvbp.NgayBatDauBp = System.DateTime.Now;
            nvbp.NgayKetThucBp = null;
            db.NhanVienBoPhans.Add(nvbp);
            db.SaveChanges();
            // Kiểm tra sự tồn tại của bản ghi
            var CVexistingRecord = db.NhanVienChucVus.FirstOrDefault(x =>
                x.MaNhanVien == model.nhanvienChucvu.MaNhanVien &&
                x.MaChucVu == model.nhanvienChucvu.MaChucVu);

            if (CVexistingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return Ok(new { data = "Bản ghi đã tồn tại." });
            }

            // Bản ghi chưa tồn tại, tiến hành tạo mới
            NhanVienChucVu nvcv = new NhanVienChucVu();
            nvcv.MaNhanVien = MaNhanVien;
            nvcv.MaChucVu = model.nhanvienChucvu.MaChucVu;
            nvcv.NgayBatDauCv = System.DateTime.Now;
            nvcv.NgayKetThucCv = null;
            db.NhanVienChucVus.Add(nvcv);
            db.SaveChanges();

            // Kiểm tra sự tồn tại của bản ghi
            var NVLexistingRecord = db.NhanVienLuongs.FirstOrDefault(x =>
                x.MaNhanVien == model.nhanvienLuong.MaNhanVien &&
                x.MucLuong == model.nhanvienLuong.MucLuong);

            if (NVLexistingRecord != null)
            {
                // Bản ghi đã tồn tại, không tạo bản ghi mới
                return Ok(new { data = "Bản ghi đã tồn tại." });
            }

            // Bản ghi chưa tồn tại, tiến hành tạo mới
            NhanVienLuong nvl = new NhanVienLuong();
            nvl.MaNhanVien = MaNhanVien;
            nvl.MucLuong = model.nhanvienLuong.MucLuong;
            nvl.NgayBatDau = System.DateTime.Now;
            nvl.NgayKetThuc = null;
            db.NhanVienLuongs.Add(nvl);
            db.SaveChanges();

            if (model.nhanvienBaohiem != null && model.nhanvienBaohiem.Count > 0)
            {
                foreach (var nvbh in model.nhanvienBaohiem)
                {
                    nvbh.MaNhanVien = MaNhanVien;
                    if (model.baohiem != null)
                    {
                        nvbh.MaBaoHiem = model.baohiem.MaBaoHiem;
                    }
                    db.NhanVienBaoHiems.Add(nvbh);
                }
                db.SaveChanges();
            }
            if (model.nhanvienPhucap != null && model.nhanvienPhucap.Count > 0)
            {
                foreach (var nvpc in model.nhanvienPhucap)
                {
                    nvpc.MaNhanVien = MaNhanVien;
                    if (model.phucap != null)
                    {
                        nvpc.MaPhuCap = model.phucap.MaPhuCap;
                    }
                    db.NhanVienPhuCaps.Add(nvpc);
                }
                db.SaveChanges();
            }
            if (model.nhanvienChucnang != null && model.nhanvienChucnang.Count > 0)
            {
                foreach (var nvcn in model.nhanvienChucnang)
                {
                    nvcn.MaNhanVien = MaNhanVien;
                    if (model.chucnang != null)
                    {
                        nvcn.MaChucNang = model.chucnang.MaChucNang;
                    }
                    db.NhanVienChucNangs.Add(nvcn);
                }
                db.SaveChanges();
            }
            return Ok(new { data = "OK" });
        }

        [Route("import-nhanvien")]
        [HttpPost]
        public IActionResult ImportNhanvien([FromBody] List<NhanVienModel> model)
        {
            try
            {
                foreach (var nhanvien in model)
                {
                    var newNhanvien = new NhanVien
                    {
                        HoTen = nhanvien.HoTen,
                        GioiTinh = nhanvien.GioiTinh,
                        NgaySinh = nhanvien.NgaySinh,
                        Cccd = nhanvien.Cccd,
                        DiaChi = nhanvien.DiaChi,
                        DienThoai = nhanvien.DienThoai,
                        TrangThai = 1
                    };

                    // Lưu newNhanvien vào CSDL
                    db.NhanViens.Add(newNhanvien);
                    var savedNhanViens = db.NhanViens.ToList();

                  
                       
                            var newTaikhoan = new TaiKhoan
                            {
                                MaNhanVien = newNhanvien.MaNhanVien,
                                Email = nhanvien.Email,
                                Password = "saiko@123",
                                XacMinhEmail = false
                            };

                            // Lưu newTaikhoan vào CSDL
                            db.TaiKhoans.Add(newTaikhoan);
                        
                    
                }

                db.SaveChanges();


                return Ok(new { data = "OK" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return BadRequest(new { error = ex.Message });
            }
        }

        [Route("update-nhanvien")]
        [HttpPost]
        public IActionResult UpdateNhanvien([FromBody] NhanVienModel model)
        {
            var obj_nhanvien = db.NhanViens.SingleOrDefault(x => x.MaNhanVien == model.nhanvien.MaNhanVien);

            obj_nhanvien.HoTen = model.nhanvien.HoTen;
            obj_nhanvien.GioiTinh = model.nhanvien.GioiTinh;
            obj_nhanvien.NgaySinh = model.nhanvien.NgaySinh;
            obj_nhanvien.Cccd = model.nhanvien.Cccd;
            obj_nhanvien.DiaChi = model.nhanvien.DiaChi;
            obj_nhanvien.HinhAnh = model.nhanvien.HinhAnh;
            obj_nhanvien.DienThoai = model.nhanvien.DienThoai;
            obj_nhanvien.MaTrinhDo = model.nhanvien.MaTrinhDo;
            obj_nhanvien.TrangThai = model.nhanvien.TrangThai;
            db.SaveChanges();

            var obj_nhanvienphongban = db.NhanVienPhongBans.FirstOrDefault(x => x.MaNhanVien == obj_nhanvien.MaNhanVien && x.NgayKetThucPb == null);
            var obj_nhanvienbophan = db.NhanVienBoPhans.FirstOrDefault(x => x.MaNhanVien == obj_nhanvien.MaNhanVien && x.NgayKetThucBp == null);
            var obj_nhanvienchucvu = db.NhanVienChucVus.FirstOrDefault(x => x.MaNhanVien == obj_nhanvien.MaNhanVien && x.NgayKetThucCv == null);
            var obj_nhanvienluong = db.NhanVienLuongs.FirstOrDefault(x => x.MaNhanVien == obj_nhanvien.MaNhanVien && x.NgayKetThuc == null);
            var obj_taikhoan = db.TaiKhoans.FirstOrDefault(x => x.MaNhanVien == obj_nhanvien.MaNhanVien);
            obj_taikhoan.Email = model.taikhoan.Email;
            obj_taikhoan.Password = model.taikhoan.Password;

            db.SaveChanges();
            // Cập nhật lịch sử phòng ban, bộ phận, chức vụ, lương
            // Kiểm tra thay đổi về MaPhongBan
            if (obj_nhanvienphongban != null && obj_nhanvienphongban.MaPhongBan != model.nhanvienPhongban.MaPhongBan)
            {
                // Thay đổi NgayKetThucPb của bản ghi cũ thành DateTime.Now
                obj_nhanvienphongban.NgayKetThucPb = System.DateTime.Now;

                // Tạo bản ghi mới chỉ tại bảng NhanVienPhongBan
                var newNhanVienPhongBan = new NhanVienPhongBan()
                {
                    MaNhanVien = obj_nhanvien.MaNhanVien,
                    MaPhongBan = model.nhanvienPhongban.MaPhongBan,
                    NgayBatDauPb = System.DateTime.Now,
                    NgayKetThucPb = null
                };
                db.NhanVienPhongBans.Add(newNhanVienPhongBan);
            }

            // Kiểm tra thay đổi về MaBoPhan
            if (obj_nhanvienbophan != null && obj_nhanvienbophan.MaBoPhan != model.nhanvienBophan.MaBoPhan)
            {
                // Thay đổi NgayKetThucBp của bản ghi cũ thành DateTime.Now
                obj_nhanvienbophan.NgayKetThucBp = System.DateTime.Now;

                // Tạo bản ghi mới chỉ tại bảng NhanVienBoPhan
                var newNhanVienBoPhan = new NhanVienBoPhan()
                {
                    MaNhanVien = obj_nhanvien.MaNhanVien,
                    MaBoPhan = model.nhanvienBophan.MaBoPhan,
                    NgayBatDauBp = System.DateTime.Now,
                    NgayKetThucBp = null
                };
                db.NhanVienBoPhans.Add(newNhanVienBoPhan);
            }

            // Kiểm tra thay đổi về MaChucVu
            if (obj_nhanvienchucvu != null && obj_nhanvienchucvu.MaChucVu != model.nhanvienChucvu.MaChucVu)
            {
                // Thay đổi NgayKetThucCv của bản ghi cũ thành DateTime.Now
                obj_nhanvienchucvu.NgayKetThucCv = System.DateTime.Now;

                // Tạo bản ghi mới chỉ tại bảng NhanVienChucVu
                var newNhanVienChucVu = new NhanVienChucVu()
                {
                    MaNhanVien = obj_nhanvien.MaNhanVien,
                    MaChucVu = model.nhanvienChucvu.MaChucVu,
                    NgayBatDauCv = System.DateTime.Now,
                    NgayKetThucCv = null
                };
                db.NhanVienChucVus.Add(newNhanVienChucVu);
            }

            // Kiểm tra thay đổi về Luong
            if (obj_nhanvienluong != null && obj_nhanvienluong.MucLuong != model.nhanvienLuong.MucLuong)
            {
                // Thay đổi NgayKetThuc của bản ghi cũ thành DateTime.Now
                obj_nhanvienluong.NgayKetThuc = System.DateTime.Now;

                // Tạo bản ghi mới chỉ tại bảng NhanVienLuong
                var newNhanVienLuong = new NhanVienLuong()
                {
                    MaNhanVien = obj_nhanvien.MaNhanVien,
                    MucLuong = model.nhanvienLuong.MucLuong,
                    NgayBatDau = System.DateTime.Now,
                    NgayKetThuc = null
                };
                db.NhanVienLuongs.Add(newNhanVienLuong);
            }

            
            var obj_nhanvienbaohiem = db.NhanVienBaoHiems.Where(x => x.MaNhanVien == obj_nhanvien.MaNhanVien);
            db.NhanVienBaoHiems.RemoveRange(obj_nhanvienbaohiem);

            // Thêm các bảo hiểm mới
            if (model.nhanvienBaohiem != null && model.nhanvienBaohiem.Count > 0)
            {
                foreach (var item in model.nhanvienBaohiem)
                {
                    item.MaNhanVien = model.nhanvien.MaNhanVien;
                    if (model.baohiem != null)
                    {
                        item.MaBaoHiem = model.baohiem.MaBaoHiem;
                    }
                    db.NhanVienBaoHiems.Add(item);
                }
                db.SaveChanges();
            }
            var obj_nhanvienphucap = db.NhanVienPhuCaps.Where(x => x.MaNhanVien == obj_nhanvien.MaNhanVien);
            db.NhanVienPhuCaps.RemoveRange(obj_nhanvienphucap);

            // Thêm các phụ cấp mới
            if (model.nhanvienPhucap != null && model.nhanvienPhucap.Count > 0)
            {
                foreach (var item in model.nhanvienPhucap)
                {
                    item.MaNhanVien = model.nhanvien.MaNhanVien;
                    if (model.phucap != null)
                    {
                        item.MaPhuCap = model.phucap.MaPhuCap;
                    }
                    db.NhanVienPhuCaps.Add(item);
                }
                db.SaveChanges();
            }

            var obj_nhanvienchucnang = db.NhanVienChucNangs.Where(x => x.MaNhanVien == obj_nhanvien.MaNhanVien);
            db.NhanVienChucNangs.RemoveRange(obj_nhanvienchucnang);

            // Thêm các chức năng mới
            if (model.nhanvienChucnang != null && model.nhanvienChucnang.Count > 0)
            {
                foreach (var item in model.nhanvienChucnang)
                {
                    item.MaNhanVien = model.nhanvien.MaNhanVien;
                    if (model.chucnang != null)
                    {
                        item.MaChucNang = model.chucnang.MaChucNang;
                    }
                    db.NhanVienChucNangs.Add(item);
                }
                db.SaveChanges();
            }
            return Ok(new { data = "OK" });
        }
    

        [Route("delete-nhanvien/{id}")]
        [HttpDelete]
        public IActionResult DeleteNhanvien(int? id)
        {
            var obj1 = db.NhanVienBaoHiems.Where(s => s.MaNhanVien == id);
            db.NhanVienBaoHiems.RemoveRange(obj1);
            db.SaveChanges();

            var obj2 = db.NhanVienPhuCaps.Where(s => s.MaNhanVien == id);
            db.NhanVienPhuCaps.RemoveRange(obj2);
            db.SaveChanges();

            var obj3 = db.NhanVienChucNangs.Where(s => s.MaNhanVien == id);
            db.NhanVienChucNangs.RemoveRange(obj3);
            db.SaveChanges();

            var obj4 = db.NhanVienPhongBans.SingleOrDefault(s => s.MaNhanVien == id);
            db.NhanVienPhongBans.Remove(obj4);
            db.SaveChanges();

            var obj5 = db.NhanVienBoPhans.SingleOrDefault(s => s.MaNhanVien == id);
            db.NhanVienBoPhans.Remove(obj5);
            db.SaveChanges();

            var obj6 = db.NhanVienChucVus.SingleOrDefault(s => s.MaNhanVien == id);
            db.NhanVienChucVus.Remove(obj6);
            db.SaveChanges();

            var obj7 = db.TaiKhoans.SingleOrDefault(s => s.MaNhanVien == id);
            db.TaiKhoans.Remove(obj7);
            db.SaveChanges();

            var obj8 = db.NhanViens.SingleOrDefault(s => s.MaNhanVien == id);
            db.NhanViens.Remove(obj8);
            db.SaveChanges();

            return Ok(new { data = "OK" });
        }
    }
}
