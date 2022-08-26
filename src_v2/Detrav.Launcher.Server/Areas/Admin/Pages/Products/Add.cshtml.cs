using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Areas.Admin.Pages.Products
{
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class AddModel : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Name")]
        public string? ProductName { get; set; }

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
                var product = new ProductModel();

                product.Name = ProductName;
                product.Description = Description;
                product.IsPublished = false;

                context.Products.Add(product);
                await context.SaveChangesAsync();
                return RedirectToPage("Edit", new { id = product.Id });
            }
            return Page();
        }
    }
}
