using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Services;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Screenshots
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class EditModel : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Name")]
        public string? ScreenshotName { get; set; }

        [Required]
        [BindProperty]
        public string? Description { get; set; }


        [BindProperty]
        public IFormFile? Data { get; set; }

        public bool IsSaved { get; set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;
        private readonly IFileService fileService;


        public EditModel(ILogger<IndexModel> logger, ApplicationDbContext context, IFileService fileService)
        {
            this.logger = logger;
            this.context = context;
            this.fileService = fileService;
        }

        public ScreenshotModel? Screenshot { get; private set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            await UpdateScreenshot(id);
            if (Screenshot == null)
            {
                return NotFound();
            }

            ScreenshotName = Screenshot.Name;
            Description = Screenshot.Description;


            return Page();
        }

        private async Task UpdateScreenshot(int id)
        {
            Screenshot = await context.Screenshots
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {



            var screenshot = await context.Screenshots.FirstOrDefaultAsync(m => m.Id == id);
            if (screenshot == null)
            {
                return NotFound();
            }

            byte[]? aData = null;
            try
            {
                if (Data != null && Data.Length > 0)
                {
                    using var ms = new MemoryStream();
                    Data.CopyTo(ms);
                    aData = ms.ToArray();
                    using Image image = Image.Load(aData);
                    var size = image.Size();
                    //if (Math.Abs(size.Height / size.Width - 1.0) > 0.01)
                    //{
                    //    ModelState.AddModelError(nameof(Data), "Icon must be image with format 1x1! The best size is 256x256!");
                    //}

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Data), ex.Message);
            }

            if (ModelState.IsValid)
            {
                screenshot.Name = ScreenshotName;
                screenshot.Description = Description;
                if (aData != null && aData.Length > 0)
                {
                    await fileService.RemoveAsync(AppConstants.COLLECTION_NAME_SCREENSHOTS, screenshot.FilePath);
                    string? fileName = Data?.FileName;
                    if (String.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = Guid.NewGuid() + ".unk";
                    }
                    else
                    {
                        fileName = Guid.NewGuid() + Path.GetExtension(fileName);
                    }
                    screenshot.FilePath = (await fileService.StoreAsync(AppConstants.COLLECTION_NAME_SCREENSHOTS, fileName, aData)).Path;
                }

                await context.SaveChangesAsync();
                IsSaved = true;
            }

            await UpdateScreenshot(id);
            if (Screenshot == null)
            {
                return NotFound();
            }


            return Page();
        }
    }
}
