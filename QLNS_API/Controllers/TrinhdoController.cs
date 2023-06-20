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
    public class TrinhdoController : Controller
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
                var result = db.TrinhDos;
                var result1 = result.OrderBy(x => x.MaTrinhDo).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var tentrinhdo = formData.Keys.Contains("tentrinhdo") ? (formData["tentrinhdo"]).ToString().Trim() : "";
                var result = db.TrinhDos;
                var kq = result.Where(x => x.TenTrinhDo.Contains(tentrinhdo)).OrderByDescending(x => x.MaTrinhDo).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
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
        [Route("get-trinhdo")]
        [HttpGet]
        public IActionResult GetTrinhdo()
        {
            var result = db.TrinhDos;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = db.TrinhDos;
            var trinhdo = result.SingleOrDefault(x => x.MaTrinhDo == id);
            return Ok(new { trinhdo });
        }

        [Route("create-trinhdo")]
        [HttpPost]
        public IActionResult CreateTrinhdo([FromBody] TrinhDoModel model)
        {
            db.TrinhDos.Add(model.trinhdo);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-trinhdo")]
        [HttpPost]
        public IActionResult UpdateTrinhdo([FromBody] TrinhDoModel model)
        {
            var obj_trinhdo = db.TrinhDos.SingleOrDefault(x => x.MaTrinhDo == model.trinhdo.MaTrinhDo);

            obj_trinhdo.TenTrinhDo = model.trinhdo.TenTrinhDo;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-trinhdo/{id}")]
        [HttpDelete]
        public IActionResult DeleteTrinhdo(int? id)
        {
            var obj = db.TrinhDos.SingleOrDefault(s => s.MaTrinhDo == id);
            db.TrinhDos.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
