using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Files
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class DeleteFileModel : PageModel
    {
        private readonly ILogger<DeleteFileModel> logger;
        private readonly ApplicationDbContext context;

        public DeleteFileModel(ILogger<DeleteFileModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public string? CollectionName { get; set; }
        public string? FilePath { get; set; }
        public int FileId { get; set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            this.FileId = id;
            var file = await context.Files.FirstOrDefaultAsync(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            CollectionName = file.Collection;
            FilePath = file.Path;
            return Page();
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {
            this.FileId = id;
            var file = await context.Files.FirstOrDefaultAsync(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            CollectionName = file.Collection;
            FilePath = file.Path;

            context.Files.Remove(file);


            await context.SaveChangesAsync();

            return RedirectToPage("ListFiles", new { collection = CollectionName });
        }
    }
}
