using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Dto;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Services;
using Detrav.Launcher.Server.Utils;
using Detrav.Launcher.Server.ViewModels;
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
            string? filePath = null,
            long fileSize = 0)
        {
            if (string.IsNullOrWhiteSpace(apikey))
                throw new ArgumentNullException(nameof(apikey));
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException(nameof(version));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (fileSize <= 0)
                throw new ArgumentNullException(nameof(fileSize));

            var product = await context.Products.FirstOrDefaultAsync(m => m.ApiKey == apikey);
            if (product == null)
            {
                return Unauthorized();
            }



            var productVersionModel = await context.Versions.FirstOrDefaultAsync(m => m.ProductId == product.Id && m.Version == version);

            if (productVersionModel == null)
            {
                productVersionModel = new ProductVersionModel()
                {
                    Version = version,
                    Product = product
                };

                context.Versions.Add(productVersionModel);
                await context.SaveChangesAsync();
            }

            var fileModel = await fileService.StoreAsync(AppConstants.COLLECTION_NAME_PRODUCT + product.Id + "/" + version, filePath, Request.Body, fileSize);

            return Ok();
        }

        [HttpGet("GetListWithInfo")]
        public async Task<ActionResult<FilesListVM>> GetListWithInfo(
            [FromHeader(Name = "X-ApiKey")] string? apikey = null,
            string? version = null)
        {
            if (string.IsNullOrWhiteSpace(apikey))
                throw new ArgumentNullException(nameof(apikey));
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException(nameof(version));

            var product = await context.Products.FirstOrDefaultAsync(m => m.ApiKey == apikey);
            if (product == null)
            {
                return Unauthorized();
            }

            var result = new FilesListVM();

            var collection = AppConstants.COLLECTION_NAME_PRODUCT + product.Id + "/" + version;

            var versionModel = await context.Versions
                .FirstOrDefaultAsync(m => m.ProductId == product.Id && m.Version == version);

            if (versionModel == null)
                return result;



            foreach (var file in context.Files.Where(m => m.Collection == collection))
            {
                var fileVM = new FileVM()
                {
                    Path = file.Path,
                    Size = file.Size,

                };
                result.Files.Add(fileVM);
                foreach (var blobDto in context.FileBlobs
                    .Include(m => m.Blob)
                    .Select(m => new FileBlobDto()
                    {
                        BlobId = m.BlobId,
                        FileId = m.FileId,
                        Hash = m.Blob!.Hash,
                        Seek = m.Seek,
                        Size = m.Blob.Size
                    })
                    .Where(m => m.FileId == file.Id)
                    .AsNoTracking())
                {
                    fileVM.Blobs.Add(new FileBlobVM()
                    {
                        Hash = blobDto.Hash,
                        Seek = blobDto.Seek,
                        Size = blobDto.Size
                    });
                }
            }

            return result;
        }
    }
}
