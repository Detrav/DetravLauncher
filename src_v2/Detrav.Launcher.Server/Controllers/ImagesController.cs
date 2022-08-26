using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Detrav.Launcher.Server.Services;
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

            var product = await context.Products.Include(m => m.Poster).FirstOrDefaultAsync(m => m.Id == id);

            if (product != null && product.Poster != null && product.Poster.Size > 0)
            {

                if (product.IsPublished || (await authorizationService.AuthorizeAsync(User, Utils.AppConstants.RequireAdministratorRole)).Succeeded)
                {
                    return File(await fileService.GetAsync(product.Poster.Id), "image/png");
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
                    .Include(m => m.Icon)
                    .Include(m => m.Product)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            else
            {
                achievement = await context.Achievements
                    .Include(m => m.Icon)
                    .Include(m => m.ProductUsers)
                    .Include(m => m.Product)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }

            if (achievement != null && achievement.Icon != null && achievement.Icon.Size > 0)
            {
                if ((achievement.Product?.IsPublished).GetValueOrDefault() 
                    && (!achievement.IsHidden || achievement.ProductUsers.Any(m => m.UserId == userId))
                    || (await authorizationService.AuthorizeAsync(User, Utils.AppConstants.RequireAdministratorRole)).Succeeded)
                {
                    return File(await fileService.GetAsync(achievement.Icon.Id), "image/png");
                }
            }

            var path = Path.GetFullPath("wwwroot/imgs/achievement_placeholder.png");
            return PhysicalFile(path, "image/png");
        }
    }
}
