using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Screenshots
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class DeleteModel : PageModel
    {
        public ScreenshotModel? Screenshot { get; private set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public DeleteModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            Screenshot = await context.Screenshots
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Screenshot == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {
            Screenshot = await context.Screenshots
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Screenshot == null)
            {
                return NotFound();
            }
            context.Screenshots.Remove(Screenshot);
            await context.SaveChangesAsync();

            return RedirectToPage("List");
        }
    }
}
