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
    public class ChucvuController : Controller
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
                var result = db.ChucVus;
                var result1 = result.OrderBy(x => x.MaChucVu).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var tenchucvu = formData.Keys.Contains("tenchucvu") ? (formData["tenchucvu"]).ToString().Trim() : "";
                var result = db.ChucVus;
                var result1 = result.Where(x => x.TenChucVu.Contains(tenchucvu)).OrderBy(x => x.MaChucVu).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
        [Route("get-chucvu")]
        [HttpGet]
        public IActionResult GetChucvu()
        {
            var result = db.ChucVus;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = db.ChucVus;
            var chucvu = result.SingleOrDefault(x => x.MaChucVu == id);
            return Ok(new { chucvu });
        }

        [Route("create-chucvu")]
        [HttpPost]
        public IActionResult CreateChucvu([FromBody] ChucVuModel model)
        {
            db.ChucVus.Add(model.chucvu);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-chucvu")]
        [HttpPost]
        public IActionResult UpdateChucvu([FromBody] ChucVuModel model)
        {
            var obj_chucvu = db.ChucVus.SingleOrDefault(x => x.MaChucVu == model.chucvu.MaChucVu);

            obj_chucvu.TenChucVu = model.chucvu.TenChucVu;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-chucvu/{id}")]
        [HttpDelete]
        public IActionResult DeleteChucvu(int? id)
        {
            var obj = db.ChucVus.SingleOrDefault(s => s.MaChucVu == id);
            db.ChucVus.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
