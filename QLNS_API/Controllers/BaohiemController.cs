using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using QLNS_API.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using QLNS_API.Entities;

namespace QLNS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaohiemController : Controller
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
                var result = db.BaoHiems;
                var result1 = result.OrderBy(x => x.MaBaoHiem).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
                var loaibaohiem = formData.Keys.Contains("loaibaohiem") ? (formData["loaibaohiem"]).ToString().Trim() : "";
                var result = db.BaoHiems;
                var result1 = result.Where(x => x.LoaiBaoHiem.Contains(loaibaohiem)).OrderBy(x => x.MaBaoHiem).Skip(pageSize * (page - 1)).Take(pageSize).ToList();

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
        [Route("get-baohiem")]
        [HttpGet]
        public IActionResult GetBaohiem()
        {
            var result = db.BaoHiems;
            return Ok(result);
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = db.BaoHiems;
            var baohiem = result.SingleOrDefault(x => x.MaBaoHiem == id);
            return Ok(new { baohiem });
        }
        [Route("get-by-manhanvien/{manhanvien}")]
        [HttpGet]
        public IActionResult GetByMaNhanVien(int? manhanvien)
        {
            var result = from bh in db.BaoHiems
                         join nvbh in db.NhanVienBaoHiems on bh.MaBaoHiem equals nvbh.MaBaoHiem
                         join nv in db.NhanViens on nvbh.MaNhanVien equals nv.MaNhanVien
                         select new
                         {
                             Id = nvbh.Id,
                             MaBaoHiem = bh.MaBaoHiem,
                             LoaiBaoHiem = bh.LoaiBaoHiem,
                             MucDong = bh.MucDong,
                             MaNhanVien = nv.MaNhanVien,
                             HoTen = nv.HoTen
                         };
            var baohiem = result.Where(x => x.MaNhanVien == manhanvien).ToList();
            return Ok(new { baohiem });
        }
        [Route("create-baohiem")]
        [HttpPost]
        public IActionResult CreateBaohiem([FromBody] BaoHiemModel model)
        {
            db.BaoHiems.Add(model.baohiem);
            db.SaveChanges();


            return Ok(new { data = "OK" });
        }


        [Route("update-baohiem")]
        [HttpPost]
        public IActionResult UpdateBaohiem([FromBody] BaoHiemModel model)
        {
            var obj_baohiem = db.BaoHiems.SingleOrDefault(x => x.MaBaoHiem == model.baohiem.MaBaoHiem);

            obj_baohiem.LoaiBaoHiem = model.baohiem.LoaiBaoHiem;
            obj_baohiem.MucDong = model.baohiem.MucDong;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

        [Route("delete-baohiem/{id}")]
        [HttpDelete]
        public IActionResult DeleteBaohiem(int? id)
        {
            var obj = db.BaoHiems.SingleOrDefault(s => s.MaBaoHiem == id);
            db.BaoHiems.Remove(obj);
            db.SaveChanges();
            return Ok(new { data = "OK" });
        }
        
    }
}
