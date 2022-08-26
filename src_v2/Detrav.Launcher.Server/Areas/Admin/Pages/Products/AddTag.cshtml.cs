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
    public class AddTagModel : PageModel
    {
        public ProductModel? Product { get; set; }
        public IEnumerable<TagModel>? Tags { get; private set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public AddTagModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            Product = await context.Products
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            Tags = await context.Tags.ToArrayAsync();

            return Page();

        }
    }
}
