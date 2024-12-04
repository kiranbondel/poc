using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Cfmg.Cafe.Manager.Application.Interfaces;
using Cfmg.Cafe.Manager.Application.Services;
using Cfmg.Cafe.Manager.Domain.Interfaces;
using Cfmg.Cafe.Manager.Infrastructure;
using Cfmg.Cafe.Manager.Infrastructure.Repositories;

namespace Cfmg.Cafe.Manager.Application
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                // Configure DbContext
                Configuration["Database:ConnectionString"] = Configuration["Database:ConnectionString"]?
                    .Replace("${SQL_SERVER}", Environment.GetEnvironmentVariable("SQL_SERVER"))
                    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
                    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
                    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

                services.AddDbContext<CafeManagerDbContext>(options =>
                    options.UseSqlServer(Configuration["Database:ConnectionString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: int.Parse(s: Configuration["Database:MaxRetryCount"] ?? string.Empty),
                            maxRetryDelay: TimeSpan.FromSeconds(int.Parse(s:Configuration["Database:MaxRetryDelaySeconds"] ?? "60")),
                            errorNumbersToAdd: null);
                    }));


                services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                });

                services.AddControllers();
                services.AddSwaggerGen();
                services.AddTransient<JwtSecurityTokenHandler>();
                services.AddScoped<IEmployeeService, EmployeeService>();
                services.AddScoped<ICafeService,CafeService>();
                services.AddScoped<ICafeRepository, CafeRepository>();
                services.AddScoped<IEmployeeRepository, EmployeeRepository>();
                services.AddHealthChecks();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error configuring services: {e}");
                throw;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization(); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
