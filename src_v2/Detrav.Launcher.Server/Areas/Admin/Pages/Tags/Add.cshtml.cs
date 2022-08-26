using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Tags
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class AddModel : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Name")]
        public string? TagName { get; set; }

        [Required]
        [BindProperty]
        public string? Description { get; set; }


        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public AddModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPostAsync()
        {


            if (ModelState.IsValid)
            {
                var tag = new TagModel();

                tag.Name = TagName;
                tag.Description = Description;
                
                context.Tags.Add(tag);
                await context.SaveChangesAsync();
                return RedirectToPage("Edit", new { id = tag.Id });
            }
            return Page();
        }
    }
}
