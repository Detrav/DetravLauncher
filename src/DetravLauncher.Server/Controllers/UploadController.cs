using DetravLauncher.Server.Models;
using DetravLauncher.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DetravLauncher.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {


        private readonly ILogger<UpdateController> _logger;
        private readonly FileProviderService fileProvider;
        private readonly MainConfigModel config;

        public UploadController(ILogger<UpdateController> logger, FileProviderService fileProvider, IOptions<MainConfigModel> options)
        {
            _logger = logger;
            this.fileProvider = fileProvider;
            this.config = options.Value;
        }

        [HttpPost("File")]
        public ActionResult PostFile([FromBody] FileUploadModel model, [FromQuery] string? apiKey = null)
        {
            if (String.IsNullOrWhiteSpace(model.Path))
                throw new ArgumentNullException(nameof(model.Path));

            if (apiKey != config.ApiKey)
            {
                return Unauthorized();
            }

            var fullPath = fileProvider.GetFile(model.Path);

            if (model.Seek == 0)
            {
                System.IO.File.WriteAllBytes(fullPath, model.Data);
            }
            else
            {
                using var stream = System.IO.File.OpenWrite(fullPath);
                stream.Position = model.Seek;
                stream.Write(model.Data, 0, model.Data.Length);
            }

            return Ok();
        }

        [HttpDelete("File")]
        public ActionResult DeleteFile([FromQuery] string? path, [FromQuery] string? apiKey = null)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            if (apiKey != config.ApiKey)
            {
                return Unauthorized();
            }

            var fullPath = fileProvider.GetFile(path);

            System.IO.File.Delete(fullPath);

            return Ok();
        }
    }
}