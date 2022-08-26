using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Pages
{
    public class ProductViewModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public ProductViewModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public ProductModel? Product { get; private set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(m=> m.Id == id);
            if (product == null || !product.IsPublished)
            {
                return NotFound();
            }

            Product = product;

            return Page();
        }
    }
}
