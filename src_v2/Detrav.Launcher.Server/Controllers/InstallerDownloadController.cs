using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Text;

namespace Detrav.Launcher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallerDownload : ControllerBase
    {
        [HttpGet("launcher.zip")]
        public ActionResult Get()
        {
            const string path = "wwwroot/files/launcher.zip";
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }


            using var sourceFile = System.IO.File.OpenRead(path);
            using ZipArchive sourceZip = new ZipArchive(sourceFile, ZipArchiveMode.Read);

            using var ms = new MemoryStream();
            using (ZipArchive targetZip = new ZipArchive(ms, ZipArchiveMode.Create))
            {

                foreach (var entry in sourceZip.Entries)
                {
                    var targetEntry = targetZip.CreateEntry(entry.FullName, CompressionLevel.Fastest);
                    using var sourceEntryStream = entry.Open();
                    using var targetEntryStream = targetEntry.Open();

                    sourceEntryStream.CopyTo(targetEntryStream);
                }
                var settingsFile = targetZip.CreateEntry("settings.txt");
                using (var settingsFileStream = settingsFile.Open())
                {
                    var url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.PathBase;
                    var bytes = Encoding.UTF8.GetBytes(url);

                    settingsFileStream.Write(bytes, 0, bytes.Length);
                }

                targetZip.CreateEntry("Extract_files_to_some_folder_before_run");
            }

            return File(ms.ToArray(), "application/zip", "launcher.zip");
        }
    }
}
