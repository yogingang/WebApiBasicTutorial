using Serilog;

namespace WebApiBasicTutorial.Extensions
{
    public static class IHostBuilderExtension
    {
        public static void AddSerilog(this IHostBuilder self)
        {
            self.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
        }
    }
}
