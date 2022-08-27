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
    public class ListModel : PageModel
    {
        private readonly ILogger<ListModel> logger;
        private readonly ApplicationDbContext context;

        public ListModel(ILogger<ListModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IEnumerable<ProductVersionModel>? Versions { get; private set; }


        public async Task OnGetAsync()
        {
            Versions = await context.Versions.ToArrayAsync();
        }
    }
}
