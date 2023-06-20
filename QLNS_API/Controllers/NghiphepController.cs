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
    public class NghiphepController : Controller
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
                var result = from np in db.NghiPheps
                             join nv in db.NhanViens on np.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 Id = np.Id,
                                 NgayBatDauNghi = np.NgayBatDauNghi,
                                 NgayKetThucNghi = np.NgayKetThucNghi,
                                 MaNhanVien = np.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var result1 = result.Where( x => ((x.NgayBatDauNghi.Month == thang || thang == null) && (x.NgayBatDauNghi.Year == nam || nam == null) )|| ((x.NgayKetThucNghi.Month == thang || thang == null) && (x.NgayKetThucNghi.Year == nam || nam == null))).OrderBy(x => x.MaNhanVien).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var result = from np in db.NghiPheps
                             join nv in db.NhanViens on np.MaNhanVien equals nv.MaNhanVien
                             select new
                             {
                                 Id = np.Id,
                                 NgayBatDauNghi = np.NgayBatDauNghi,
                                 NgayKetThucNghi = np.NgayKetThucNghi,
                                 MaNhanVien = np.MaNhanVien,
                                 HoTen = nv.HoTen
                             };
                var result1 = result.Where(x => x.HoTen.Contains(hoten) && ( (x.NgayBatDauNghi.Month == thang || thang == null) && (x.NgayBatDauNghi.Year == nam || nam == null) || (x.NgayKetThucNghi.Month == thang || thang == null) && (x.NgayKetThucNghi.Year == nam || nam == null))).OrderBy(x => x.MaNhanVien).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
        [Route("get-nghiphep")]
        [HttpGet]
        public IActionResult GetNghiphep()
        {
            var result = db.NghiPheps;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = from np in db.NghiPheps
                         join nv in db.NhanViens on np.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = np.Id,
                             NgayBatDauNghi = np.NgayBatDauNghi,
                             NgayKetThucNghi = np.NgayKetThucNghi,
                             MaNhanVien = np.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var nghiphep = result.SingleOrDefault(x => x.Id == id);
            
            return Ok(new { nghiphep });
        }

        [Route("create-nghiphep")]
        [HttpPost]
        public IActionResult CreateNghiphep([FromBody] NghiPhepModel model)
        {
            db.NghiPheps.Add(model.nghiphep);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-nghiphep")]
        [HttpPost]
        public IActionResult UpdateNghiphep([FromBody] NghiPhepModel model)
        {
            var obj_nghiphep = db.NghiPheps.SingleOrDefault(x => x.Id == model.nghiphep.Id);

            obj_nghiphep.NgayBatDauNghi = model.nghiphep.NgayBatDauNghi;
            obj_nghiphep.NgayKetThucNghi = model.nghiphep.NgayKetThucNghi;
            obj_nghiphep.MaNhanVien = model.nghiphep.MaNhanVien;


            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-nghiphep/{id}")]
        [HttpDelete]
        public IActionResult DeleteNghiphep(int? id)
        {
            var obj = db.NghiPheps.SingleOrDefault(s => s.Id == id);
            db.NghiPheps.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
        private int CalculateNumberOfDays(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            // Tính số ngày phép từ ngày bắt đầu và ngày kết thúc
            TimeSpan duration = ngayKetThuc.Date - ngayBatDau.Date;
            return duration.Days + 1;
        }
    }
}
