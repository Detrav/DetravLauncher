using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVersionController : ControllerBase
    {

        private readonly ILogger<ProductVersionController> logger;
        private readonly ApplicationDbContext context;
        private readonly IFileService fileService;

        public ProductVersionController(ILogger<ProductVersionController> logger, ApplicationDbContext context, IFileService fileService)
        {
            this.logger = logger;
            this.context = context;
            this.fileService = fileService;
        }

        [HttpPost("UploadFile")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> UploadFile(
            [FromHeader(Name = "X-ApiKey")] string? apikey = null,
            string? version = null,
            string? filePath = null)
        {
            if (string.IsNullOrWhiteSpace(apikey))
                throw new ArgumentNullException(nameof(apikey));
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException(nameof(version));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var product = await context.Products.FirstOrDefaultAsync(m => m.ApiKey == apikey);
            if (product == null)
            {
                return Unauthorized();
            }

            var fileModel = await fileService.StoreWithAutoSaveAsync("Product" + product.Id, filePath, Request.Body);

            var productVersionModel = await context.Versions.FirstOrDefaultAsync(m => m.ProductId == product.Id && m.Version == version);

            if (productVersionModel == null)
            {
                productVersionModel = new ProductVersionModel()
                {
                    Product = product
                };

                context.Versions.Add(productVersionModel);
            }

            ProductVersionFileModel productVersionFileModel = new ProductVersionFileModel();
            productVersionFileModel.File = fileModel;
            productVersionFileModel.Version = productVersionModel;

            context.VersionFiles.Add(productVersionFileModel);

            return Ok();
        }
    }
}
