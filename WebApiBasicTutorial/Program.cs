using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApiBasicTutorial.Behaviors;
using WebApiBasicTutorial.Extensions;
using WebApiBasicTutorial.Helper;
using WebApiBasicTutorial.Infrastructure;
using WebApiBasicTutorial.Injectables;
using WebApiBasicTutorial.Middleware;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.InitControllerAndSwagger();
    builder.Host.AddSerilog();

    builder.Services.AddMediatR(AssemblyHelper.GetAllAssemblies().ToArray());
    builder.Services.InitMediatR();
    builder.Services.AddAccessDb(builder.Configuration.GetConnectionString("DbServer"));
    builder.Services.InitScrutor();

    var app = builder.Build();
    app.UseSwaggerAndSwaggerUI();
    app.UseMiddleware<ApiLoggingMiddleware>();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.OrdinalIgnoreCase)) throw;
    Log.Fatal(ex, "Host terminated unexpectedly");
}finally
{
    Log.CloseAndFlush();
}