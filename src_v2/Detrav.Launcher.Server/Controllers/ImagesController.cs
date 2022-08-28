using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Services;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Detrav.Launcher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly ILogger<AdminActionsController> logger;
        private readonly ApplicationDbContext context;
        private readonly IAuthorizationService authorizationService;
        private readonly IFileService fileService;
        private readonly UserManager<IdentityUser> userManager;

        public ImagesController(
            ILogger<AdminActionsController> logger,
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            IFileService fileService,
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.context = context;
            this.authorizationService = authorizationService;
            this.fileService = fileService;
            this.userManager = userManager;
        }

        [HttpGet("GetPosterFromProduct/{id}")]
        public async Task<ActionResult> GetPosterFromProduct(int id)
        {

            var product = await context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product != null && product.PosterFilePath != null)
            {
                if (product.IsPublished || (await authorizationService.AuthorizeAsync(User, Utils.AppConstants.RequireAdministratorRole)).Succeeded)
                {
                    var file = await fileService.GetOrDefaultAsync(AppConstants.COLLECTION_NAME_POSTERS, product.PosterFilePath);
                    if (file != null)
                    {
                        return File(file, "image/png");
                    }
                }
            }
            var path = Path.GetFullPath("wwwroot/imgs/product_placeholder.png");
            return PhysicalFile(path, "image/png");
        }

        [HttpGet("GetIconFromAchievement/{id}")]
        public async Task<ActionResult> GetIconFromAchievement(int id)
        {
            AchievementModel? achievement;
            var userId = userManager.GetUserId(User);
            if (String.IsNullOrWhiteSpace(userId))
            {
                achievement = await context.Achievements
                    .Include(m => m.Product)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            else
            {
                achievement = await context.Achievements
                    .Include(m => m.ProductUsers)
                    .Include(m => m.Product)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }

            if (achievement != null && achievement.IconFilePath != null)
            {
                if ((achievement.Product?.IsPublished).GetValueOrDefault()
                    && (!achievement.IsHidden || achievement.ProductUsers.Any(m => m.UserId == userId))
                    || (await authorizationService.AuthorizeAsync(User, Utils.AppConstants.RequireAdministratorRole)).Succeeded)
                {
                    var file = await fileService.GetOrDefaultAsync(AppConstants.COLLECTION_NAME_ACHIEVEMENTS, achievement.IconFilePath);
                    if (file != null)
                        return File(file, "image/png");
                }
            }

            var path = Path.GetFullPath("wwwroot/imgs/achievement_placeholder.png");
            return PhysicalFile(path, "image/png");
        }

        [HttpGet("GetDataFromScreenshot/{id}")]
        public async Task<ActionResult> GetDataFromScreenshot(int id)
        {
            ScreenshotModel? screenshot;
            var userId = userManager.GetUserId(User);

            screenshot = await context.Screenshots
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (screenshot != null && screenshot.FilePath != null)
            {
                if ((screenshot.Product?.IsPublished).GetValueOrDefault()
                    || (await authorizationService.AuthorizeAsync(User, Utils.AppConstants.RequireAdministratorRole)).Succeeded)
                {
                    var file = await fileService.GetOrDefaultAsync(AppConstants.COLLECTION_NAME_SCREENSHOTS, screenshot.FilePath);
                    if (file != null)
                        return File(file, "image/png");
                }
            }

            var path = Path.GetFullPath("wwwroot/imgs/achievement_placeholder.png");
            return PhysicalFile(path, "image/png");
        }
    }
}
