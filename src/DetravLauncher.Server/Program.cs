using DetravLauncher.Server.Models;
using DetravLauncher.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MainConfigModel>(builder.Configuration.GetSection("MainConfig"));
builder.Services.AddSingleton<FileProviderService>();

var app = builder.Build();

var config = app.Services.GetRequiredService<MainConfigModel>();


if (config.ContentPath == null)
    throw new NullReferenceException(nameof(config.ContentPath));

if (!Directory.Exists(config.ContentPath))
    Directory.CreateDirectory(config.ContentPath);

if (!String.IsNullOrWhiteSpace(config.BasePath))
    app.UsePathBase(config.BasePath);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
