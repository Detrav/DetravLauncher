using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Services;
using Detrav.Launcher.Server.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server
{
    public class Program
    {
        private static bool isMSSQLServer = /*builder.Environment.IsProduction()*/ true;
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Configure(builder);
            var app = builder.Build();
            await Build(app);
            app.UseAuthentication(); ;
            await app.RunAsync();
        }

        private static async Task Build(WebApplication app)
        {
            if (isMSSQLServer)
            // Migrations
            {
                var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await context.Database.MigrateAsync();
                }
            }

            app.UsePathBase("/testPath");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();
        }

        private static void Configure(WebApplicationBuilder builder)
        {
            if (isMSSQLServer)
            {
                builder.Services.AddHostedService<FileServiceWatchDogMSSQL>();
                // Add services to the container.
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {

                    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                    options.UseSqlServer(connectionString);

                });
            }
            else
            {
                builder.Services.AddHostedService<FileServiceWatchDogInMemory>();
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("testdb");
                });
            }
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();


            builder.Services.AddAuthorization(options =>
            {
                if (builder.Environment.IsProduction())
                {
                    options.AddPolicy(AppConstants.RequireAdministratorRole,
                         policy => policy.RequireRole("Administrator"));
                }
                else
                {
                    options.AddPolicy(AppConstants.RequireAdministratorRole, policy => policy.RequireAuthenticatedUser());
                }
            });

            builder.Services.AddScoped<IFileService, FileService>();
        }
    }
}