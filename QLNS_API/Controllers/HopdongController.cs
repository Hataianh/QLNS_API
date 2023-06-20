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
    public class HopdongController : Controller
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
                var result = from hd in db.HopDongs
                             join lhd in db.LoaiHopDongs on hd.MaLoaiHopDong equals lhd.MaLoaiHopDong
                             join nv in db.NhanViens on hd.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 MaHopDong = hd.MaHopDong,
                                 MaLoaiHopDong = lhd.MaLoaiHopDong,
                                 TenLoaiHopDong = lhd.TenLoaiHopDong,
                                 NgayKy = hd.NgayKy,
                                 NoiDung = hd.NoiDung,
                                 LanKy = hd.LanKy,
                                 MaNhanVien = hd.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var result1 = result.Where(x => (x.NgayKy.HasValue && x.NgayKy.Value.Month == thang || thang == null) && (x.NgayKy.Value.Year == nam || nam == null)).OrderBy(x => x.MaHopDong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                int? thang = null;
                int? nam = null;
                if (formData.Keys.Contains("nam") && !string.IsNullOrEmpty(Convert.ToString(formData["nam"]))) { nam = int.Parse(formData["nam"].ToString()); }
                if (formData.Keys.Contains("thang") && !string.IsNullOrEmpty(Convert.ToString(formData["thang"]))) { thang = int.Parse(formData["thang"].ToString()); }
                var hoten = formData.Keys.Contains("hoten") ? (formData["hoten"]).ToString().Trim() : "";
                var result = from hd in db.HopDongs
                             join lhd in db.LoaiHopDongs on hd.MaLoaiHopDong equals lhd.MaLoaiHopDong
                             join nv in db.NhanViens on hd.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 MaHopDong = hd.MaHopDong,
                                 MaLoaiHopDong = lhd.MaLoaiHopDong,
                                 TenLoaiHopDong = lhd.TenLoaiHopDong,
                                 NgayKy = hd.NgayKy,
                                 NoiDung = hd.NoiDung,
                                 LanKy = hd.LanKy,
                                 MaNhanVien = hd.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var kq = result.Where(x => x.HoTen.Contains(hoten) && (x.NgayKy.HasValue && x.NgayKy.Value.Month == thang || thang == null) && (x.NgayKy.Value.Year == nam || nam == null)).OrderByDescending(x => x.MaHopDong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                return Ok(
                    new Pagination
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
        [Route("get-hopdong")]
        [HttpGet]
        public IActionResult GetHopdong()
        {
            var result = db.HopDongs;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = from hd in db.HopDongs
                         join lhd in db.LoaiHopDongs on hd.MaLoaiHopDong equals lhd.MaLoaiHopDong
                         join nv in db.NhanViens on hd.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             MaHopDong = hd.MaHopDong,
                             MaLoaiHopDong = lhd.MaLoaiHopDong,
                             TenLoaiHopDong = lhd.TenLoaiHopDong,
                             NgayKy = hd.NgayKy,
                             NoiDung = hd.NoiDung,
                             LanKy = hd.LanKy,
                             MaNhanVien = hd.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var hopdong = result.SingleOrDefault(x => x.MaHopDong == id);
            return Ok(new { hopdong });
        }

        [Route("create-hopdong")]
        [HttpPost]
        public IActionResult CreateHopdong([FromBody] HopDongModel model)
        {
            db.HopDongs.Add(model.hopdong);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-hopdong")]
        [HttpPost]
        public IActionResult UpdateHopdong([FromBody] HopDongModel model)
        {
            var obj_hopdong = db.HopDongs.SingleOrDefault(x => x.MaHopDong == model.hopdong.MaHopDong);

            obj_hopdong.MaLoaiHopDong = model.hopdong.MaLoaiHopDong;
            obj_hopdong.NgayKy = model.hopdong.NgayKy;
            obj_hopdong.NoiDung = model.hopdong.NoiDung;
            obj_hopdong.LanKy = model.hopdong.LanKy;
            obj_hopdong.MaNhanVien = model.hopdong.MaNhanVien;


            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-hopdong/{id}")]
        [HttpDelete]
        public IActionResult DeleteHopdong(int? id)
        {
            var obj = db.HopDongs.SingleOrDefault(s => s.MaHopDong == id);
            db.HopDongs.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
