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
    public class EditModel : PageModel
    {

        [Display(Name = "Name")]
        public string? VersionName { get; set; }
        [BindProperty]
        public bool IsBeta { get; set; }
        [BindProperty]
        public bool IsPublished { get; set; }
        public bool IsSaved { get; set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public EditModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public ProductVersionModel? Version { get; private set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            await UpdateVersion(id);
            if (Version == null)
            {
                return NotFound();
            }

            VersionName = Version.Version;
            IsBeta = Version.IsBeta;
            IsPublished = Version.IsPublished;

            return Page();
        }

        private async Task UpdateVersion(int id)
        {
            Version = await context.Versions
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {

            var version = await context.Versions.FirstOrDefaultAsync(m => m.Id == id);
            if (version == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                version.IsBeta = IsBeta;
                version.IsPublished = IsPublished;
                await context.SaveChangesAsync();
                IsSaved = true;
            }

            await UpdateVersion(id);
            if (Version == null)
            {
                return NotFound();
            }

            VersionName = Version.Version;
            IsBeta = Version.IsBeta;
            IsPublished = Version.IsPublished;


            return Page();
        }
    }
}
