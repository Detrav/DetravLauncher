using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Files
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class DeleteCollectionModel : PageModel
    {
        private readonly ILogger<DeleteCollectionModel> logger;
        private readonly ApplicationDbContext context;

        public DeleteCollectionModel(ILogger<DeleteCollectionModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public string? CollectionName { get; private set; }

        public Task OnGetAsync(string collection)
        {
            collection = Uri.UnescapeDataString(collection);
            CollectionName = collection;
            return Task.CompletedTask;
        }

        public async Task<ActionResult> OnPostAsync(string collection)
        {


            foreach (var file in context.Files.Where(m => m.Collection == collection))
            {
                context.Files.Remove(file);
            }

            await context.SaveChangesAsync();

            return RedirectToPage("ListCollections");
        }
    }
}
