using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Tags
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class EditModel : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Name")]
        public string? TagName { get; set; }

        [Required]
        [BindProperty]
        public string? Description { get; set; }

        public bool IsSaved { get; set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public EditModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public TagModel? Tag { get; private set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            await UpdateTag(id);
            if (Tag == null)
            {
                return NotFound();
            }

            TagName = Tag.Name;
            Description = Tag.Description;

            return Page();
        }

        private async Task UpdateTag(int id)
        {
            Tag = await context.Tags
                .Include(m => m.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {

            var tag = await context.Tags.FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                tag.Name = TagName;
                tag.Description = Description;
                await context.SaveChangesAsync();
                IsSaved = true;
            }

            await UpdateTag(id);
            if (Tag == null)
            {
                return NotFound();
            }


            return Page();
        }
    }
}
