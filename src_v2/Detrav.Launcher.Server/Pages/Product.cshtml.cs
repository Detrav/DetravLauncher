using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Pages
{
    public class ProductViewModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public ProductViewModel(ILogger<IndexModel> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
        }

        public ProductModel? Product { get; set; }
        public string? UserId { get; set; }
        public ProductUserModel? ProductUser { get; private set; }
        public ProductUserLibraryModel? ProductUserLibrary { get; set; }

        public bool IsSaved { get; set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            var product = await context.Products
                .Include(m => m.Screenshots)
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null || !product.IsPublished)
            {
                return NotFound();
            }

            Product = product;

            UserId = userManager.GetUserId(User);

            if (!String.IsNullOrEmpty(UserId))
            {
                ProductUser = await context.ProductUsers.FirstOrDefaultAsync(m => m.UserId == UserId);
                if (ProductUser != null)
                {
                    ProductUserLibrary = await context.ProductUserLibraries.FirstOrDefaultAsync(m => m.ProductId == product.Id && m.UserId == ProductUser.Id);
                }
            }


            return Page();
        }

        public async Task<ActionResult> OnPostAddToLibraryAsync(int id)
        {
            var product = await context.Products
                .Include(m => m.Screenshots)
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null || !product.IsPublished)
            {
                return NotFound();
            }

            Product = product;
            UserId = userManager.GetUserId(User);

            if (!String.IsNullOrEmpty(UserId))
            {
                ProductUser = await context.ProductUsers.FirstOrDefaultAsync(m => m.UserId == UserId);
                if (ProductUser == null)
                {
                    ProductUser = new ProductUserModel()
                    {
                        UserId = UserId,
                    };
                    context.ProductUsers.Add(ProductUser);
                    await context.SaveChangesAsync();
                }

                ProductUserLibrary = await context.ProductUserLibraries.FirstOrDefaultAsync(m => m.ProductId == product.Id && m.UserId == ProductUser.Id);
                if (ProductUserLibrary == null)
                {
                    ProductUserLibrary = new ProductUserLibraryModel()
                    {
                        ProductId = product.Id,
                        UserId = ProductUser.Id
                    };
                    context.ProductUserLibraries.Add(ProductUserLibrary);
                }


                if (Product.DistributionType == Data.Enums.ProductDistributionType.Free)
                {
                    ProductUserLibrary.IsOwner = true;
                }
                else if (Product.DistributionType == Data.Enums.ProductDistributionType.Paid)
                {
                    ProductUserLibrary.IsRequest = true;
                }

                await context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
