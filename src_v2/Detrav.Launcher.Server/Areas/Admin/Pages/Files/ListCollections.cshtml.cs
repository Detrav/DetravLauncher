using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Dto;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Files
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class ListCollectionsModel : PageModel
    {
        private readonly ILogger<ListCollectionsModel> logger;
        private readonly ApplicationDbContext context;

        public ListCollectionsModel(ILogger<ListCollectionsModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public FileCollectionDto[]? Items { get; private set; }

        public async Task OnGetAsync()
        {
            Items = await context.Files
                .GroupBy(m => m.Collection)
                .Select(m => new FileCollectionDto { Collection = m.Key, Count = m.Count() }).ToArrayAsync();
        }
    }
}
