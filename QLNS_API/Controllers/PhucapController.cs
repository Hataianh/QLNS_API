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
    public class PhucapController : Controller
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
                var result = db.PhuCaps;
                var result1 = result.OrderBy(x => x.MaPhuCap).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var tenphucap = formData.Keys.Contains("tenphucap") ? (formData["tenphucap"]).ToString().Trim() : "";
                var result = db.PhuCaps;
                var kq = result.Where(x => x.TenPhuCap.Contains(tenphucap)).OrderBy(x => x.MaPhuCap).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
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
        [Route("get-phucap")]
        [HttpGet]
        public IActionResult GetPhucap()
        {
            var result = db.PhuCaps;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = db.PhuCaps;
            var phucap = result.SingleOrDefault(x => x.MaPhuCap == id);
            return Ok(new { phucap });
        }
        [Route("get-by-manhanvien/{manhanvien}")]
        [HttpGet]
        public IActionResult GetByMaNhanVien(int? manhanvien)
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
            return Ok(new { phucap, tongphucap });
        }
        [Route("create-phucap")]
        [HttpPost]
        public IActionResult CreatePhucap([FromBody] PhuCapModel model)
        {
            db.PhuCaps.Add(model.phucap);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-phucap")]
        [HttpPost]
        public IActionResult UpdatePhucap([FromBody] PhuCapModel model)
        {
            var obj_phucap = db.PhuCaps.SingleOrDefault(x => x.MaPhuCap == model.phucap.MaPhuCap);

            obj_phucap.TenPhuCap = model.phucap.TenPhuCap;
            obj_phucap.SoTien = model.phucap.SoTien;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-phucap/{id}")]
        [HttpDelete]
        public IActionResult DeletePhucap(int? id)
        {
            var obj = db.PhuCaps.SingleOrDefault(s => s.MaPhuCap == id);
            db.PhuCaps.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
        
    }
}
