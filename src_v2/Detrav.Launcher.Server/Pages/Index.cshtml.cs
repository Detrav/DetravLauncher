using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext context;

        public IEnumerable<ProductModel>? Products { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task OnGetAsync()
        {
            Products = await context.Products.Where(m => m.IsPublished).ToArrayAsync();


            //var result = new List<ProductModel>();
            //for (int i = 0; i < 99; i++)
            //{
            //    result.Add(new ProductModel()
            //    {
            //        Name = "Test Product"
            //    });
            //}
            //Products = result;

        }
    }
}