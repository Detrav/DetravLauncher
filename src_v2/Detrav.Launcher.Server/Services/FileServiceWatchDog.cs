using Detrav.Launcher.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Detrav.Launcher.Server.Services
{
    public class FileServiceWatchDogMSSQL : BackgroundService
    {
        private IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<FileServiceWatchDogMSSQL> logger;

        public FileServiceWatchDogMSSQL(IServiceScopeFactory serviceScopeFactory, ILogger<FileServiceWatchDogMSSQL> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    int rows = await context.Database.ExecuteSqlRawAsync(@"
DELETE [dbo].[Blobs]
  FROM [dbo].[Blobs] AS b  
  LEFT JOIN [dbo].[FileBlobs] AS fb ON b.Id = fb.BlobId
  WHERE fb.FileId IS NULL
  ;
");
                    logger.LogInformation("Collect garbage {number} blobs from files table!", rows);
                }

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }

    public class FileServiceWatchDogInMemory : BackgroundService
    {
        private IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<FileServiceWatchDogInMemory> logger;

        public FileServiceWatchDogInMemory(IServiceScopeFactory serviceScopeFactory, ILogger<FileServiceWatchDogInMemory> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    logger.LogInformation("Collect garbage from file blobs!");
                    //await context.Database.ExecuteSqlInterpolatedAsync($"");
                }

                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }
    }
}
