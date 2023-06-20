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
    public class TaikhoanController : Controller
    {
        private QLNSContext db = new QLNSContext();

        
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(int? id)
        {
            var result = from tk in db.TaiKhoans
                         join nv in db.NhanViens on tk.MaNhanVien equals nv.MaNhanVien
                         select new TaiKhoan
                         {
                             Email = tk.Email,
                             Password = tk.Password,
                             MaNhanVien = tk.MaNhanVien,
                         };
            var taikhoan = result.SingleOrDefault(x => x.MaNhanVien == id);
            return Ok(new { taikhoan });
        }

        [Route("update-taikhoan")]
        [HttpPost]
        public IActionResult UpdateTaikhoan([FromBody] TaiKhoanModel model)
        {
            var obj_taikhoan = db.TaiKhoans.SingleOrDefault(x => x.MaNhanVien == model.taikhoan.MaNhanVien);

            obj_taikhoan.Email = model.taikhoan.Email;
            obj_taikhoan.Password = model.taikhoan.Password;

            db.SaveChanges();


            return Ok(new { data = "OK" });
        }

    }
}
