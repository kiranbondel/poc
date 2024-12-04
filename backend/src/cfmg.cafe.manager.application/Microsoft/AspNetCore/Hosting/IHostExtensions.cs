using Microsoft.EntityFrameworkCore;

namespace Cfmg.Cafe.Manager.Application.Microsoft.AspNetCore.Hosting
{
    public static class IHostExtensions
    {
        public static IHost Migrate<TContext>(this IHost host, Action<TContext, IServiceProvider>? successfulCallBack = null) where TContext : DbContext
        {
            return host.Migrate(false, successfulCallBack);
        }

        public static IHost Migrate<TContext>(this IHost host, bool isSeedingEnabled, Action<TContext, IServiceProvider>? successfulCallBack = null) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var dbContext = services.GetService<TContext>();

            try
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var commandTimeout = configuration.GetValue("Database:CommandTimeout", 300); // Default 5 minutes

                var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();

                if (pendingMigrations.Count > 0)
                {
                    foreach (var pendingMigration in pendingMigrations)
                    {
                        logger.LogInformation("Pending migration: {Migration}", pendingMigration);
                    }
                }
                else
                {
                    logger.LogInformation("No pending migrations for {DbContextName}", typeof(TContext).Name);
                }

                dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(commandTimeout));
                logger.LogInformation("Starting migration for {DbContextName}", typeof(TContext).Name);
                dbContext.Database.Migrate();
                logger.LogInformation("Migration for {DbContextName} completed", typeof(TContext).Name);

                var appliedMigrations = dbContext.Database.GetAppliedMigrations();
                logger.LogInformation("Applied migrations:");
                foreach (var migration in appliedMigrations)
                {
                    logger.LogInformation("{AppliedMigration}", migration);
                }

                successfulCallBack?.Invoke(dbContext, services);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Migration for {DbContextName} failed: {ErrorMessage}", typeof(TContext).Name, ex.Message);
                throw;
            }

            return host;
        }
    }
}
