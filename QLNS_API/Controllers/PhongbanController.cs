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
    public class PhongbanController : Controller
    {
        private QLNSContext db = new QLNSContext();

        [Route("loadData")]
        [HttpPost]
        public IActionResult Boloc([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                var result = db.PhongBans;
                var result1 = result.OrderBy(x => x.MaPhongBan).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                
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
                var tenphongban = formData.Keys.Contains("tenphongban") ? (formData["tenphongban"]).ToString().Trim() : "";
                var result = db.PhongBans;
                var result1 = result.Where(x => x.TenPhongBan.Contains(tenphongban)).OrderBy(x => x.MaPhongBan).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
        [Route("get-phongban")]
        [HttpGet]
        public IActionResult GetPhongban()
        {
            var result = db.PhongBans;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = db.PhongBans;
            var phongban = result.SingleOrDefault(x => x.MaPhongBan == id);
            return Ok(new { phongban });
        }

        [Route("create-phongban")]
        [HttpPost]
        public IActionResult CreatePhongban([FromBody] PhongBanModel model)
        {
            db.PhongBans.Add(model.phongban);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-phongban")]
        [HttpPost]
        public IActionResult UpdatePhongban([FromBody] PhongBanModel model)
        {
            var obj_phongban = db.PhongBans.SingleOrDefault(x => x.MaPhongBan == model.phongban.MaPhongBan);

            obj_phongban.TenPhongBan = model.phongban.TenPhongBan;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-phongban/{id}")]
        [HttpDelete]
        public IActionResult DeletePhongban(int? id)
        {
            var obj = db.PhongBans.SingleOrDefault(s => s.MaPhongBan == id);
            db.PhongBans.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
