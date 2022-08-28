using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Files
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class ListFilesModel : PageModel
    {
        private readonly ILogger<ListFilesModel> logger;
        private readonly ApplicationDbContext context;

        public ListFilesModel(ILogger<ListFilesModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public FileModel[]? Items { get; private set; }
        public string? CollectionName { get; private set; }

        public async Task OnGetAsync(string collection)
        {
            collection = Uri.UnescapeDataString(collection);
            CollectionName = collection;
            Items = await context.Files
                .Where(m => m.Collection == collection)
                .ToArrayAsync();
        }
    }
}
