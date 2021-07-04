namespace ComicTracker.Web
{
    using System.Reflection;

    using ComicTracker.Data;
    using ComicTracker.Data.Common;
    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models;
    using ComicTracker.Data.Repositories;
    using ComicTracker.Data.Seeding;
    using ComicTracker.Services.Data;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Mapping;
    using ComicTracker.Services.Messaging;
    using ComicTracker.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ComicTrackerDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ComicTrackerDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services

            // ComicTracker.Services.Data
            services.AddTransient<IArcDetailsService, ArcDetailsService>();
            services.AddTransient<IGenreRetrievalService, GenreRetrievalService>();
            services.AddTransient<IHomePageService, HomePageService>();
            services.AddTransient<IIssueDetailsService, IssueDetailsService>();
            services.AddTransient<IListService, ListService>();
            services.AddTransient<ISeriesCreationService, SeriesCreationService>();
            services.AddTransient<ISeriesDetailsService, SeriesDetailsService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IVolumeDetailsService, VolumeDetailsService>();

            // ComicTracker.Services.Messaging
            services.AddTransient<IEmailSender, NullMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ComicTrackerDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        // Custom routing for SeriesController so that it supports the following route:
                        // Series/{id?}; Example: Series/1 => Directs us to Series with Id of 1;
                        // Much more convenient than using query string "id=1"
                        // Needs a constraint that makes sure the id is a number.
                        // Otherwise, it blocks out other actions,
                        // eg. Series/Create since it treats "Creats" as a non-existent id.
                        endpoints.MapControllerRoute(
                            name: "details",
                            pattern: "{controller}/{id}",
                            defaults: new { action = "Index" },
                            constraints: new { id = @"\d+" });

                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
