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
    public class LoaihopdongController : Controller
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
                var result = db.LoaiHopDongs;
                var result1 = result.OrderBy(x => x.MaLoaiHopDong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var tenloaihopdong = formData.Keys.Contains("tenloaihopdong") ? (formData["tenloaihopdong"]).ToString().Trim() : "";
                var result = db.LoaiHopDongs;
                var kq = result.Where(x => x.TenLoaiHopDong.Contains(tenloaihopdong)).OrderByDescending(x => x.MaLoaiHopDong).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
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
        [Route("get-loaihopdong")]
        [HttpGet]
        public IActionResult GetLoaihopdong()
        {
            var result = db.LoaiHopDongs;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = db.LoaiHopDongs;
            var loaihopdong = result.SingleOrDefault(x => x.MaLoaiHopDong == id);
            return Ok(new { loaihopdong });
        }

        [Route("create-loaihopdong")]
        [HttpPost]
        public IActionResult CreateLoaihopdong([FromBody] LoaiHopDongModel model)
        {
            db.LoaiHopDongs.Add(model.loaihopdong);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-loaihopdong")]
        [HttpPost]
        public IActionResult UpdateLoaihopdong([FromBody] LoaiHopDongModel model)
        {
            var obj_loaihopdong = db.LoaiHopDongs.SingleOrDefault(x => x.MaLoaiHopDong == model.loaihopdong.MaLoaiHopDong);

            obj_loaihopdong.TenLoaiHopDong = model.loaihopdong.TenLoaiHopDong;
            obj_loaihopdong.MauHopDong = model.loaihopdong.MauHopDong;


            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-loaihopdong/{id}")]
        [HttpDelete]
        public IActionResult DeleteLoaihopdong(int? id)
        {
            var obj = db.LoaiHopDongs.SingleOrDefault(s => s.MaLoaiHopDong == id);
            db.LoaiHopDongs.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
