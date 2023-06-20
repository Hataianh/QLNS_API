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
    public class BophanController : Controller
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
                var result = db.BoPhans;
                var result1 = result.OrderBy(x => x.MaBoPhan).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var tenbophan = formData.Keys.Contains("tenbophan") ? (formData["tenbophan"]).ToString().Trim() : "";
                var result = db.BoPhans;
                var result1 = result.Where(x => x.TenBoPhan.Contains(tenbophan)).OrderBy(x => x.MaBoPhan).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
        [Route("get-bophan")]
        [HttpGet]
        public IActionResult GetBophan()
        {
            var result = db.BoPhans;
            return Ok(result);
        }
        [Route("get-bophanmpb")]
        [HttpGet]
        public IActionResult GetBophanmpb(int? maphongban)
        {
            var result = db.BoPhans.Where(x => x.MaPhongBan == maphongban).ToList();
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = from bp in db.BoPhans
                         join pb in db.PhongBans on bp.MaPhongBan equals pb.MaPhongBan
                         select new
                         {
                             MaBoPhan = bp.MaBoPhan,
                             TenBoPhan = bp.TenBoPhan,
                             MaPhongBan = bp.MaPhongBan,
                             TenPhongBan = pb.TenPhongBan
                         };
            var bophan = result.SingleOrDefault(x => x.MaBoPhan == id);
            return Ok(new { bophan });
        }
        
        [Route("create-bophan")]
        [HttpPost]
        public IActionResult CreateBophan([FromBody] BoPhanModel model)
        {
            db.BoPhans.Add(model.bophan);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-bophan")]
        [HttpPost]
        public IActionResult UpdateBophan([FromBody] BoPhanModel model)
        {
            var obj_bophan = db.BoPhans.SingleOrDefault(x => x.MaBoPhan == model.bophan.MaBoPhan);

            obj_bophan.TenBoPhan = model.bophan.TenBoPhan;
            obj_bophan.MaPhongBan = model.bophan.MaPhongBan;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-bophan/{id}")]
        [HttpDelete]
        public IActionResult DeleteBophan(int? id)
        {
            var obj = db.BoPhans.SingleOrDefault(s => s.MaBoPhan == id);
            db.BoPhans.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
    }
}
