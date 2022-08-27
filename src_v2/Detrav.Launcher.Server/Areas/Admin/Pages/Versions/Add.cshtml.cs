using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Versions
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class AddModel : PageModel
    {

        public int ProductId { get; set; }
        public string? ApiKey { get; set; }


        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public AddModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<ActionResult> OnGetAsync(int productId)
        {
            this.ProductId = productId;
            var product = await context.Products.FirstOrDefaultAsync(m => m.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            if (String.IsNullOrWhiteSpace(product.ApiKey))
            {
                product.ApiKey = Guid.NewGuid().ToString();
                await context.SaveChangesAsync();
            }

            ApiKey = product.ApiKey;

            return Page();
        }

        public async Task<ActionResult> OnPostAsync(int productId)
        {
            this.ProductId = productId;
            var product = await context.Products.FirstOrDefaultAsync(m => m.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            product.ApiKey = Guid.NewGuid().ToString();
            await context.SaveChangesAsync();

            ApiKey = product.ApiKey;

            return Page();
        }

    }
}
