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


namespace Detrav.Launcher.Server.Areas.Admin.Pages.Products
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class EditModel : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Name")]
        public string? ProductName { get; set; }

        [Required]
        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public bool IsPublished { get; set; }

        [BindProperty]
        public IFormFile? Poster { get; set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;
        private readonly IFileService fileService;

        public EditModel(ILogger<IndexModel> logger, ApplicationDbContext context, IFileService fileService)
        {
            this.logger = logger;
            this.context = context;
            this.fileService = fileService;
        }

        public ProductModel? Product { get; private set; }
        public bool IsSaved { get; private set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            await UpdateProduct(id);
            if (Product == null)
            {
                return NotFound();
            }

            ProductName = Product.Name;
            Description = Product.Description;
            IsPublished = Product.IsPublished;

            return Page();
        }

        private async Task UpdateProduct(int id)
        {
            Product = await context.Products
                .Include(m => m.Tags)
                .Include(m => m.Screenshots)
                .Include(m => m.Versions)
                .Include(m => m.Achievements)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {
            byte[]? aPoster = null;
            try
            {
                if (Poster != null && Poster.Length > 0)
                {
                    using var ms = new MemoryStream();
                    Poster.CopyTo(ms);
                    aPoster = ms.ToArray();
                    using Image image = Image.Load(aPoster);
                    var size = image.Size();
                    if (Math.Abs((float)size.Height / (float)size.Width - 1.6) > 0.1)
                    {
                        ModelState.AddModelError(nameof(Poster), "Poster must be image with format 10x16! The best size is 500x800!");
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Poster), ex.Message);
            }
            var product = await context.Products.Include(m => m.Poster).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                product.Name = ProductName;
                product.Description = Description;
                product.IsPublished = IsPublished;
                if (aPoster != null && aPoster.Length > 0)
                {
                    await fileService.RemoveAsync(product.Poster);
                    product.Poster = await fileService.StoreAsync(Poster!.FileName, aPoster);
                }
                await context.SaveChangesAsync();
                IsSaved = true;
            }

            await UpdateProduct(id);
            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
