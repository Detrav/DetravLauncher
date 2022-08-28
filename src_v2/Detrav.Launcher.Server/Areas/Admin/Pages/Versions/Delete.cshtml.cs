using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Versions
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class DeleteModel : PageModel
    {
        public ProductVersionModel? Version { get; private set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public DeleteModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            Version = await context.Versions
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Version == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {
            Version = await context.Versions
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Version == null)
            {
                return NotFound();
            }

            var collection = AppConstants.COLLECTION_NAME_PRODUCT + Version.ProductId + "/" + Version.Version;

            context.Versions.Remove(Version);
            foreach (var file in context.Files.Where(m => m.Collection == collection))
            {
                context.Files.Remove(file);
            }

            await context.SaveChangesAsync();

            return RedirectToPage("List");
        }
    }
}
