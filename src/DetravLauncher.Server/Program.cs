using DetravLauncher.Server.Models;
using DetravLauncher.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(

        lo =>

        lo.Protocols = HttpProtocols.Http1AndHttp2

        );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MainConfigModel>(builder.Configuration.GetSection("MainConfig"));
builder.Services.AddSingleton<FileProviderService>();

var app = builder.Build();

var config = app.Services.GetRequiredService<IOptions<MainConfigModel>>();


if (!String.IsNullOrWhiteSpace(config.Value.BasePath))
{
    Console.WriteLine("Use base path: " + config.Value.BasePath);
    app.UsePathBase(config.Value.BasePath);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
