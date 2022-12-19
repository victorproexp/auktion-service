global using auktionAPI.Models;
global using auktionAPI.Services;
using NLog;
using NLog.Web;
using Microsoft.OpenApi.Models;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.Configure<AuktionDatabaseSettings>(
        builder.Configuration.GetSection("AuktionDatabase"));
    
    builder.Services.AddSingleton<IAuktionService, AuktionService>();
    builder.Services.AddSingleton<IBudHandler, BudHandler>();

    builder.Services.AddRazorPages();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "auktionAPI",
            Description = "En ASP.NET Core Web API til at håndtere auktioner",
        });
    });

    builder.Services.AddHostedService<AuktionWorker>();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.MapRazorPages();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
