using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AppConstants.RequireAdministratorRole)]
    public class AdminActionsController : ControllerBase
    {

        private readonly ILogger<AdminActionsController> logger;
        private readonly ApplicationDbContext context;

        public AdminActionsController(ILogger<AdminActionsController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }


        [HttpPost("AddTagToProduct")]
        public async Task<ActionResult> AddTagToProduct(int productId = 0, int tagId = 0)
        {
            var tag = await context.Tags
               .FirstOrDefaultAsync(m => m.Id == tagId);

            if (tag == null)
            {
                return NotFound();
            }

            var product = await context.Products
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            product.Tags.Add(tag);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("RemoveTagFromProduct")]
        public async Task<ActionResult> RemoveTagFromProduct(int productId = 0, int tagId = 0)
        {
            var tag = await context.Tags
               .FirstOrDefaultAsync(m => m.Id == tagId);

            if (tag == null)
            {
                return NotFound();
            }

            var product = await context.Products
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            product.Tags.Remove(tag);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
