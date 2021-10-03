using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OdeToFood.Data;

namespace OdeToFood
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                services.AddHttpsRedirection(options => { options.HttpsPort = 443; });
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
                
                var connectionString = Environment.GetEnvironmentVariable("OdeToFoodDBProduction");
                services.AddDbContextPool<OdeToFoodDbContext>(options =>
                {
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                        .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                        .EnableDetailedErrors(); // <-- with debugging (remove for production).
                });
            }
            else
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 25));
            
                services.AddDbContextPool<OdeToFoodDbContext>(options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("OdeToFoodDB"), serverVersion)
                        .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                        .EnableDetailedErrors(); // <-- with debugging (remove for production).
                });
            }
            
            // services.AddSingleton<IRestaurantData, InMemoryRestaurantData>();
            services.AddScoped<IRestaurantData, SqlRestaurantData>();
            services.AddRazorPages();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app
                    .UseForwardedHeaders()
                    .UseHttpsRedirection();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNodeModules();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
