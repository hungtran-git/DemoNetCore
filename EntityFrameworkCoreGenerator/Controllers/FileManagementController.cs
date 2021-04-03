using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCoreGenerator.Controllers
{
    public class DLLFileModel {
        public string DllName { get; set; }
        public IFormFile DllFile { get; set; }
        public List<IFormFile> DllFiles { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class FileManagementController : ControllerBase
    {
        private readonly ILogger<FileManagementController> _logger;
        private IConfiguration _configuration;
        private IWebHostEnvironment _webHostEnvironment;

        public FileManagementController(ILogger<FileManagementController> logger, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _logger                 = logger;
            _configuration          = configuration;
            _webHostEnvironment     = webHostEnvironment;
        }

        [Route("PostFiles")]
        public async Task<IActionResult> PostFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }

        [Route("PostFile")]
        public async Task<IActionResult> PostFile(IFormFile formFile)
        {
            if (formFile.Length > 0)
            {
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath,
                    _configuration["AppSetting:StoredFilesPath"],
                    formFile.FileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await formFile.CopyToAsync(stream);
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok();
        }

        [Route("PostFormData")]
        public async Task<IActionResult> PostFormData([FromForm]DLLFileModel formData)
        {
            var file = formData.DllFile;
            var fileContentType = formData.DllFile.ContentType;
            var fileName = formData.DllFile.FileName;
            byte[] byteArray = null;
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = byteArray = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                }
            }

            return File(new MemoryStream(byteArray), fileContentType, fileName);
        }
    }
}
