using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using QLNS_API.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using QLNS_API.Entities;

namespace QLNS_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KhenthuongkyluatController : Controller
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
                var result = from kt in db.KhenThuongKyLuats
                             join nv in db.NhanViens on kt.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 Id = kt.Id,
                                 NoiDung = kt.NoiDung,
                                 NgayQuyetDinh = kt.NgayQuyetDinh,
                                 SoTien = kt.SoTien,
                                 Loai = kt.Loai,
                                 MaNhanVien = kt.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var result1 = result.Where(x=> (x.NgayQuyetDinh.Month == thang || thang == null) && (x.NgayQuyetDinh.Year == nam || nam == null)).OrderBy(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var result = from kt in db.KhenThuongKyLuats
                             join nv in db.NhanViens on kt.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 Id = kt.Id,
                                 NoiDung = kt.NoiDung,
                                 NgayQuyetDinh = kt.NgayQuyetDinh,
                                 SoTien = kt.SoTien,
                                 Loai = kt.Loai,
                                 MaNhanVien = kt.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var kq = result.Where(x => x.HoTen.Contains(hoten) && (x.NgayQuyetDinh.Month == thang || thang == null) && (x.NgayQuyetDinh.Year == nam || nam == null)).OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
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
        [Route("get-khenthuongkyluat")]
        [HttpGet]
        public IActionResult GetKhenthuongkyluat()
        {
            var result = db.KhenThuongKyLuats;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = from kt in db.KhenThuongKyLuats
                         join nv in db.NhanViens on kt.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = kt.Id,
                             NoiDung = kt.NoiDung,
                             NgayQuyetDinh = kt.NgayQuyetDinh,
                                 SoTien = kt.SoTien,
                             Loai = kt.Loai,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var khenthuongkyluat = result.SingleOrDefault(x => x.Id == id);
            return Ok(new { khenthuongkyluat });
        }
        [Route("get-by-manhanvien/{manhanvien}")]
        [HttpGet]
        public IActionResult GetByMaNhanVien(int? manhanvien)
        {
            var result = from kt in db.KhenThuongKyLuats
                         join nv in db.NhanViens on kt.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = kt.Id,
                             NoiDung = kt.NoiDung,
                             NgayQuyetDinh = kt.NgayQuyetDinh,
                                 SoTien = kt.SoTien,
                             Loai = kt.Loai,
                             MaNhanVien = kt.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var khenthuongkyluat = result.Where(x => x.MaNhanVien == manhanvien).ToList();
            return Ok(new { khenthuongkyluat });
        }

        [Route("create-khenthuongkyluat")]
        [HttpPost]
        public IActionResult CreateKhenthuongkyluat([FromBody] KhenThuongKyLuatModel model)
        {
            db.KhenThuongKyLuats.Add(model.khenthuongkyluat);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("update-khenthuongkyluat")]
        [HttpPost]
        public IActionResult UpdateKhenthuongkyluat([FromBody] KhenThuongKyLuatModel model)
        {
            var obj_khenthuongkyluat = db.KhenThuongKyLuats.SingleOrDefault(x => x.Id == model.khenthuongkyluat.Id);

            obj_khenthuongkyluat.NoiDung = model.khenthuongkyluat.NoiDung;
            obj_khenthuongkyluat.NgayQuyetDinh = model.khenthuongkyluat.NgayQuyetDinh;
            obj_khenthuongkyluat.SoTien = model.khenthuongkyluat.SoTien;
            obj_khenthuongkyluat.Loai = model.khenthuongkyluat.Loai;
            obj_khenthuongkyluat.MaNhanVien = model.khenthuongkyluat.MaNhanVien;


            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-khenthuongkyluat/{id}")]
        [HttpDelete]
        public IActionResult DeleteKhenthuongkyluat(int? id)
        {
            var obj = db.KhenThuongKyLuats.SingleOrDefault(s => s.Id == id);
            db.KhenThuongKyLuats.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
