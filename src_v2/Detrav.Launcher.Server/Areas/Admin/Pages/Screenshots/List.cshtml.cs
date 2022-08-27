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
    public class ListModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public ListModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IEnumerable<ScreenshotModel>? Screenshots { get; private set; }


        public async Task OnGetAsync()
        {
            Screenshots = await context.Screenshots.ToArrayAsync();
        }
    }
}
