using Ionic.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShoesHuntBackup.Data;
using ShoesHuntBackup.Data.Entities;
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
        private ShoesHuntBackupContext _shoesHuntBackupContext;

        public FileManagementController(ILogger<FileManagementController> logger, 
            IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment,
            ShoesHuntBackupContext shoesHuntBackupContext)
        {
            _logger                 = logger;
            _configuration          = configuration;
            _webHostEnvironment     = webHostEnvironment;
            _shoesHuntBackupContext = shoesHuntBackupContext;
        }

        [Route("PostFiles")]
        [HttpPost]
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
        [HttpPost]
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
        [HttpPost]
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
                    await file.CopyToAsync(ms);
                    var fileBytes = byteArray = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                }
            }

            return File(new MemoryStream(byteArray), fileContentType, fileName);
        }

        [Route("BackupDllFile")]
        [HttpPost]
        public async Task<IActionResult> BackupDllFile([FromForm] DLLFileModel formData)
        {
            var dllFile = formData.DllFile;
            var dllContentType = formData.DllFile.ContentType;
            var dllFileName = formData.DllFile.FileName;
            if (dllFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await dllFile.CopyToAsync(ms);

                    var dllModel = new DllFile { 
                        DllName = dllFileName,
                        DllContentType = dllContentType,
                        DllData = ms.ToArray(),
                        CreatedDate = DateTime.Now.ToString("s"),
                        UpdatedDate = DateTime.Now.ToString("s"),
                    };
                    _shoesHuntBackupContext.DllFiles.Add(dllModel);
                    _shoesHuntBackupContext.SaveChanges();
                }
            }
            var newRecord = _shoesHuntBackupContext.DllFiles.OrderByDescending(_=>_.Id).FirstOrDefault();
            return File(newRecord.DllData, newRecord.DllContentType);
        }

        [Route("GetDllFile")]
        [HttpGet]
        public async Task<IActionResult> GetDllFile()
        {
            var allDllFile = _shoesHuntBackupContext.DllFiles.OrderByDescending(_ => _.Id).ToList();

            var outputStream = new MemoryStream();

            using (var zip = new ZipFile())
            {
                foreach (var dllFile in allDllFile)
                {
                    zip.AddEntry(dllFile.Id+dllFile.DllName, dllFile.DllData);
                }
                zip.Save(outputStream);
            }
            outputStream.Position = 0;
            return File(outputStream, "application/zip", "filename.zip");
        }

        [Route("GetDllFileAndExtract")]
        [HttpGet]
        public async Task<IActionResult> GetDllFileAndExtract()
        {
            FileStreamResult zipFile = await GetDllFile() as FileStreamResult;

            if (zipFile.ContentType == "application/zip") {
                using (var zip = ZipFile.Read(zipFile.FileStream))
                {
                    var filePath = Path.Combine(_webHostEnvironment.ContentRootPath,
                    _configuration["AppSetting:ExtractFilesPath"]);
                    zip.ExtractAll(filePath);
                }
            }

            return Ok();
        }
    }
}
