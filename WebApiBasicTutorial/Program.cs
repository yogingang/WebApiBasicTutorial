using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApiBasicTutorial.Behaviors;
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
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
    builder.Services.AddMediatR(AssemblyHelper.GetAllAssemblies().ToArray());
    var connectionString = builder.Configuration.GetConnectionString("DbServer");
    builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRLoggingBehavior<,>));

    builder.Services.Scan(scan => scan
                            .FromAssemblies(AssemblyHelper.GetAllAssemblies())
                            .AddClasses(classes => classes.AssignableTo<ITransientService>())
                            .AsImplementedInterfaces()
                            .WithTransientLifetime()
                            .AddClasses(classes => classes.AssignableTo<IScopedService>())
                            .AsImplementedInterfaces()
                            .WithScopedLifetime()
                            .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                            .AsImplementedInterfaces()
                            .WithSingletonLifetime());

    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
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