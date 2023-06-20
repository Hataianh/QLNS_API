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
    public class ChucnangController : Controller
    {
        private QLNSContext db = new QLNSContext();

        [Route("search")]
        [HttpPost]
        public IActionResult Search([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                var tenchucnang = formData.Keys.Contains("tenchucnang") ? (formData["tenchucnang"]).ToString().Trim() : "";
                var result = db.ChucNangs;
                var kq = result.Where(x => x.TenChucNang.Contains(tenchucnang)).OrderByDescending(x => x.MaChucNang).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                return Ok(kq);

            }
            catch (Exception)
            {
                return Ok("Err");
            }
        }
        [Route("get-chucnang")]
        [HttpGet]
        public IActionResult GetChucnang()
        {
            var result = db.ChucNangs;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = db.ChucNangs;
            var chucnang = result.SingleOrDefault(x => x.MaChucNang == id);
            return Ok(new { chucnang });
        }

        [Route("create-chucnang")]
        [HttpPost]
        public IActionResult CreateChucnang([FromBody] ChucNangModel model)
        {
            db.ChucNangs.Add(model.chucnang);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-chucnang")]
        [HttpPost]
        public IActionResult UpdateChucnang([FromBody] ChucNangModel model)
        {
            var obj_chucnang = db.ChucNangs.SingleOrDefault(x => x.MaChucNang == model.chucnang.MaChucNang);

            obj_chucnang.TenChucNang = model.chucnang.TenChucNang;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-chucnang/{id}")]
        [HttpDelete]
        public IActionResult DeleteChucnang(int? id)
        {
            var obj = db.ChucNangs.SingleOrDefault(s => s.MaChucNang == id);
            db.ChucNangs.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
        
    }
}
