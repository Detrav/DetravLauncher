using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Tags
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class AddProductModel : PageModel
    {
        public TagModel? Tag { get; set; }
        public IEnumerable<ProductModel>? Products { get; private set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public AddProductModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            Tag = await context.Tags
                .Include(m => m.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Tag == null)
            {
                return NotFound();
            }

            Products = await context.Products.ToArrayAsync();

            return Page();

        }
    }
}
