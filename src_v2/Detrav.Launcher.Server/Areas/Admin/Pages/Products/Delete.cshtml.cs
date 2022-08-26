using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Products
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class DeleteModel : PageModel
    {
        public ProductModel? Product { get; private set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public DeleteModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            await UpdateProduct(id);
            if (Product == null)
            {
                return NotFound();
            }

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
            Product = await context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            context.Products.Remove(Product);
            await context.SaveChangesAsync();

            return RedirectToPage("List");
        }
    }
}
