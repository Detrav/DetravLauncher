using DetravLauncher.Server.Models;
using DetravLauncher.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DetravLauncher.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateController : ControllerBase
    {


        private readonly ILogger<UpdateController> _logger;
        private readonly FileProviderService fileProvider;

        public UpdateController(ILogger<UpdateController> logger, FileProviderService fileProvider)
        {
            _logger = logger;
            this.fileProvider = fileProvider;
        }

        [HttpGet("List/{name}")]
        public ActionResult<FileListModel> GetList(string name)
        {
            var result = new FileListModel();
            result.Files.AddRange(fileProvider.GetList(name));
            return result;
        }

        [HttpGet("File")]
        public ActionResult GetFile(string? path = null)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            var file = fileProvider.GetFile(path);

            return PhysicalFile(file, "application/octet-stream");
        }
    }
}