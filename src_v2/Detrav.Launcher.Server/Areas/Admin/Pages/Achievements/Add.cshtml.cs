using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Achievements
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class AddModel : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Name")]
        public string? AchievementName { get; set; }

        [Required]
        [BindProperty]
        public string? Description { get; set; }

        
        public int ProductId { get; set; }


        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;

        public AddModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<ActionResult> OnGetAsync(int productId)
        {
            ProductId = productId;
            var product = await context.Products
               .FirstOrDefaultAsync(m => m.Id == productId);

            if (product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<ActionResult> OnPostAsync(int productId)
        {
            ProductId = productId;
            var product = await context.Products
               .Include(m => m.Achievements)
               .FirstOrDefaultAsync(m => m.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var Achievement = new AchievementModel();

                Achievement.Name = AchievementName;
                Achievement.Description = Description;

                product.Achievements.Add(Achievement);
                await context.SaveChangesAsync();
                return RedirectToPage("Edit", new { id = Achievement.Id });
            }
            return Page();
        }
    }
}
