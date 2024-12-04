using Cfmg.Cafe.Manager.Application.Microsoft.AspNetCore.Hosting;
using Cfmg.Cafe.Manager.Application.DataSeed;
using Cfmg.Cafe.Manager.Infrastructure;

namespace Cfmg.Cafe.Manager.Application
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

			var isMigrationEnabled = Convert.ToBoolean(Environment.GetEnvironmentVariable("RUN_MIGRATIONS"));
            var isSeedingDataEnabled = Convert.ToBoolean(Environment.GetEnvironmentVariable("SEED_DATA"));

            var host = CreateHostBuilder(args).Build();

            if (isMigrationEnabled)
            {
                StartMigration(host, isSeedingDataEnabled);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void StartMigration(IHost host, bool isSeedingDataEnabled)
        {
            host.Migrate<CafeManagerDbContext>((context, sp) =>
            {
                if (isSeedingDataEnabled)
                {
                    CafeManagerDataSeed.InsertData(context);
                }
            });
        }
    }
}
