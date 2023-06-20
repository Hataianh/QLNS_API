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
    public class UngluongController : Controller
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
                var result1 = result. Where(x => (x.Ngay.Month == thang || thang == null) && (x.Ngay.Year == nam || nam == null)).OrderByDescending(x => x.Ngay).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var kq = result.Where(x => x.HoTen.Contains(hoten) && (x.Ngay.Month == thang || thang == null) && (x.Ngay.Year == nam || nam == null)).OrderByDescending(x => x.Ngay).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
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
        [Route("get-ungluong")]
        [HttpGet]
        public IActionResult GetUngluong()
        {
            var result = db.UngLuongs;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
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
            var ungluong = result.SingleOrDefault(x => x.Id == id);
            return Ok(new { ungluong });
        }

        [Route("create-ungluong")]
        [HttpPost]
        public IActionResult CreateUngluong([FromBody] UngLuongModel model)
        {
            db.UngLuongs.Add(model.ungluong);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-ungluong")]
        [HttpPost]
        public IActionResult UpdateUngluong([FromBody] UngLuongModel model)
        {
            var obj_ungluong = db.UngLuongs.SingleOrDefault(x => x.Id == model.ungluong.Id);

            obj_ungluong.Ngay = model.ungluong.Ngay;
            obj_ungluong.NoiDung = model.ungluong.NoiDung;
            obj_ungluong.SoTien = model.ungluong.SoTien;
            obj_ungluong.TrangThai = model.ungluong.TrangThai;
            obj_ungluong.MaNhanVien = model.ungluong.MaNhanVien;


            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-ungluong/{id}")]
        [HttpDelete]
        public IActionResult DeleteUngluong(int? id)
        {
            var obj = db.UngLuongs.SingleOrDefault(s => s.Id == id);
            db.UngLuongs.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
