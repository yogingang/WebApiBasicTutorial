using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiBasicTutorial.Behaviors;
using WebApiBasicTutorial.Helper;
using WebApiBasicTutorial.Infrastructure;
using WebApiBasicTutorial.Injectables;

namespace WebApiBasicTutorial.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void InitControllerAndSwagger(this IServiceCollection self)
        {
            self.AddControllers();
            self.AddEndpointsApiExplorer();
            self.AddSwaggerGen();
        }
        public static void InitMediatR(this IServiceCollection self)
        {
            self.AddMediatR(AssemblyHelper.GetAllAssemblies().ToArray());
            self.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRLoggingBehavior<,>));
        }

        public static void AddAccessDb(this IServiceCollection self, string connectionString)
        {
            self.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));
        }

        public static void InitScrutor(this IServiceCollection self)
        {
            self.Scan(scan => scan
                                .FromAssemblies(AssemblyHelper.GetAllAssemblies())
                                .AddClasses(classes => classes.AssignableTo<ITransientService>())
                                .AsImplementedInterfaces()
                                .WithTransientLifetime()
                                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                                .AsImplementedInterfaces()
                                .WithScopedLifetime()
                                .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                                .AsImplementedInterfaces()
                                .WithSingletonLifetime()
                                );
        }

    }
}
