using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using WebApi.Services;
using System.Collections.Generic;

namespace Admin_Api_BanMayTinh.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private IConfiguration _configuration;
        public UploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadInfo model)
        {
            try
            {
                if (model.file.Length > 0)
                {
                    string filePath = $"assets/images/{model.folder}/{model.file.FileName}";
                    var fullPath = CreatePathFile(filePath);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.file.CopyToAsync(fileStream);
                    }
                    return Ok(new { filePath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Không thể upload file");
            }
        }
        [Route("upload-single")]
        [HttpPost]
        public async Task<IActionResult> UploadSingle([FromForm] string folder, [FromForm] IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string filePath = $"assets/images/{folder}/{file.FileName}";
                    var fullPath = CreatePathFile(filePath);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Ok(new { filePath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Không thể upload file");
            }
        }
        [Route("upload-multi")]
        [HttpPost]
        public async Task<IActionResult> UploadMulti([FromForm] IFormFile[] files, [FromForm] string folder)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        string filePath = $"images/{folder}/{file.FileName}";
                        var fullPath = CreatePathFile(filePath);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        list.Add(filePath);

                    }
                }
                return Ok(new { list });
            }
            catch (Exception)
            {
                return StatusCode(500, "Không thể upload file");
            }
        }
        [NonAction]
        private string CreatePathFile(string RelativePathFileName)
        {
            try
            {
                string serverRootPathFolder = _configuration["AppSettings:WEB_SERVER_FULL_PATH"].ToString();
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
                if (!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                return fullPathFile;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    public class FileUploadInfo
    {
        public IFormFile file { get; set; }
        public string folder { get; set; }
        public string Description { get; set; }
    }
}
