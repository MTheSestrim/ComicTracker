namespace ComicTracker.Web
{
    using ComicTracker.Data;
    using ComicTracker.Data.Common;
    using ComicTracker.Data.Models;
    using ComicTracker.Data.Seeding;
    using ComicTracker.Services.Data.Arc;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Genre;
    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Issue;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.List;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Series;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Volume;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Services.Messaging;
    using ComicTracker.Web.Infrastructure;

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

            // AutoMapper
            services.AddAutoMapper(typeof(WebMappingProfile), typeof(ServiceMappingProfile));

            // Application services

            // ComicTracker.Services.Data
            // Arc
            services.AddTransient<IArcDetailsService, ArcDetailsService>();
            services.AddTransient<IArcRatingService, ArcRatingService>();
            services.AddTransient<IArcTemplateCreationService, ArcTemplateCreationService>();

            // Genre
            services.AddTransient<IGenreRetrievalService, GenreRetrievalService>();

            // Issue
            services.AddTransient<IIssueDetailsService, IssueDetailsService>();
            services.AddTransient<IIssueRatingService, IssueRatingService>();
            services.AddTransient<IIssueTemplateCreationService, IssueTemplateCreationService>();

            // List
            services.AddTransient<IListService, ListService>();

            // Series
            services.AddTransient<ISeriesCreationService, SeriesCreationService>();
            services.AddTransient<ISeriesDeletionService, SeriesDeletionService>();
            services.AddTransient<ISeriesDetailsService, SeriesDetailsService>();
            services.AddTransient<ISeriesEditingInfoService, SeriesEditingInfoService>();
            services.AddTransient<ISeriesEditingService, SeriesEditingService>();
            services.AddTransient<ISeriesRatingService, SeriesRatingService>();
            services.AddTransient<ISeriesSearchQueryingService, SeriesSearchQueryingService>();

            // Volume
            services.AddTransient<IVolumeCreationService, VolumeCreationService>();
            services.AddTransient<IVolumeDetailsService, VolumeDetailsService>();
            services.AddTransient<IVolumeRatingService, VolumeRatingService>();
            services.AddTransient<IVolumeTemplateCreationService, VolumeTemplateCreationService>();

            // ComicTracker.Services.Messaging
            services.AddTransient<IEmailSender, NullMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                        /* Custom routing for SeriesController so that it supports the following route:
                         Series/{id?}; Example: Series/1 => Directs us to Series with Id of 1;
                         Much more convenient than using query string "id=1"
                         Needs a constraint that makes sure the id is a number.
                         Otherwise, it blocks out other actions,
                         eg. Series/Create since it treats "Creats" as a non-existent id.*/
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
