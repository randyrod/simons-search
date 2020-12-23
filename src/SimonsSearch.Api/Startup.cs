using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Repositories;
using SimonsSearch.Core.Services;
using SimonsSearch.Data;

namespace SimonsSearch.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<ISimonsSearchDataContext, SimonsSearchDataContext>();

            services.AddHttpClient<ISimonsSearchHttpClient, SimonsSearchHttpClient>();

            services.AddScoped<ISimonsSearchDataSeeder, SimonsSearchDataSeeder>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<ILockRepository, LockRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMediumRepository, MediumRepository>();
            services.AddScoped<IBuildingWeightCalculator, BuildingWeightCalculator>();
            services.AddScoped<IBuildingSearchService, BuildingSearchService>();
            services.AddScoped<ILockWeightCalculator, LockWeightCalculator>();
            services.AddScoped<ILockSearchService, LockSearchService>();
            services.AddScoped<IGroupWeightCalculator, GroupWeightCalculator>();
            services.AddScoped<IGroupSearchService, GroupSearchService>();
            services.AddScoped<IMediumWeightCalculator, MediumWeightCalculator>();
            services.AddScoped<IMediumSearchService, MediumSearchService>();
            services.AddScoped<ISearchService, SearchService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<ISimonsSearchDataSeeder>();

                service.SeedDataAsync().Wait();
            }
        }
    }
}