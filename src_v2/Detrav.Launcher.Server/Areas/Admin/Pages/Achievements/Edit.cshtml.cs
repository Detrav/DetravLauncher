using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Services;
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
    public class EditModel : PageModel
    {

        [Required]
        [BindProperty]
        [Display(Name = "Name")]
        public string? AchievementName { get; set; }

        [Required]
        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public bool IsHidden { get; set; }

        [BindProperty]
        public IFormFile? Icon { get; set; }

        public bool IsSaved { get; set; }

        private readonly ILogger<IndexModel> logger;
        private readonly ApplicationDbContext context;
        private readonly IFileService fileService;


        public EditModel(ILogger<IndexModel> logger, ApplicationDbContext context, IFileService fileService)
        {
            this.logger = logger;
            this.context = context;
            this.fileService = fileService;
        }

        public AchievementModel? Achievement { get; private set; }

        public async Task<ActionResult> OnGetAsync(int id)
        {
            await UpdateAchievement(id);
            if (Achievement == null)
            {
                return NotFound();
            }

            AchievementName = Achievement.Name;
            Description = Achievement.Description;
            IsHidden = Achievement.IsHidden;


            return Page();
        }

        private async Task UpdateAchievement(int id)
        {
            Achievement = await context.Achievements
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ActionResult> OnPostAsync(int id)
        {



            var achievement = await context.Achievements.FirstOrDefaultAsync(m => m.Id == id);
            if (achievement == null)
            {
                return NotFound();
            }

            byte[]? aIcon = null;
            try
            {
                if (Icon != null && Icon.Length > 0)
                {
                    using var ms = new MemoryStream();
                    Icon.CopyTo(ms);
                    aIcon = ms.ToArray();
                    using Image image = Image.Load(aIcon);
                    var size = image.Size();
                    if (Math.Abs(size.Height / size.Width - 1.0) > 0.01)
                    {
                        ModelState.AddModelError(nameof(Icon), "Icon must be image with format 1x1! The best size is 256x256!");
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Icon), ex.Message);
            }

            if (ModelState.IsValid)
            {
                achievement.Name = AchievementName;
                achievement.Description = Description;
                achievement.IsHidden = IsHidden;
                if (aIcon != null && aIcon.Length > 0)
                {
                    await fileService.RemoveAsync(AppConstants.COLLECTION_NAME_ACHIEVEMENTS, achievement.IconFilePath);
                    string? fileName = Icon?.FileName;
                    if (String.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = Guid.NewGuid() + ".unk";
                    }
                    else
                    {
                        fileName = Guid.NewGuid() + Path.GetExtension(fileName);
                    }
                    achievement.IconFilePath = (await fileService.StoreAsync(AppConstants.COLLECTION_NAME_ACHIEVEMENTS, fileName, aIcon)).Path;
                }

                await context.SaveChangesAsync();
                IsSaved = true;
            }

            await UpdateAchievement(id);
            if (Achievement == null)
            {
                return NotFound();
            }


            return Page();
        }
    }
}
