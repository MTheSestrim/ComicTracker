namespace ComicTracker.Web
{
    using ComicTracker.Data;
    using ComicTracker.Data.Models;
    using ComicTracker.Data.Seeding;
    using ComicTracker.Services;
    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data;
    using ComicTracker.Services.Data.Arc;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Contracts;
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

            // Memory Cache
            services.AddMemoryCache();

            // AutoMapper
            services.AddAutoMapper(typeof(WebMappingProfile), typeof(ServiceMappingProfile));

            // Application services

            // ComicTracker.Services
            services.AddSingleton<ICacheKeyHolderService<int>, CacheKeyHolderService>();
            services.AddScoped<IEntityNameExtractService, EntityNameExtractService>();

            // ComicTracker.Services.Data
            services.AddTransient<IFileUploadService, FileUploadService>();

            // Arc
            services.AddTransient<IArcAttachmentService, ArcAttachmentService>();
            services.AddTransient<IArcCreationService, ArcCreationService>();
            services.AddTransient<IArcDeletionService, ArcDeletionService>();
            services.AddTransient<IArcDetachmentService, ArcDetachmentService>();
            services.AddTransient<IArcDetailsService, ArcDetailsService>();
            services.AddTransient<IArcEditingInfoService, ArcEditingInfoService>();
            services.AddTransient<IArcEditingService, ArcEditingService>();
            services.AddTransient<IArcRatingService, ArcRatingService>();
            services.AddTransient<IArcTemplateCreationService, ArcTemplateCreationService>();

            // Genre
            services.AddTransient<IGenreCreationService, GenreCreationService>();
            services.AddTransient<IGenreDeletionService, GenreDeletionService>();
            services.AddTransient<IGenreRetrievalService, GenreRetrievalService>();

            // Issue
            services.AddTransient<IIssueAttachmentService, IssueAttachmentService>();
            services.AddTransient<IIssueCreationService, IssueCreationService>();
            services.AddTransient<IIssueDeletionService, IssueDeletionService>();
            services.AddTransient<IIssueDetailsService, IssueDetailsService>();
            services.AddTransient<IIssueEditingInfoService, IssueEditingInfoService>();
            services.AddTransient<IIssueEditingService, IssueEditingService>();
            services.AddTransient<IIssueRatingService, IssueRatingService>();
            services.AddTransient<IIssueTemplateCreationService, IssueTemplateCreationService>();

            // List
            services.AddTransient<IListService, ListService>();

            // Series
            services.AddTransient<ISeriesCreationService, SeriesCreationService>();
            services.AddTransient<ISeriesDeletionService, SeriesDeletionService>();
            services.AddTransient<IIssueDetachmentService, IssueDetachmentService>();
            services.AddTransient<ISeriesDetailsService, SeriesDetailsService>();
            services.AddTransient<ISeriesEditingInfoService, SeriesEditingInfoService>();
            services.AddTransient<ISeriesEditingService, SeriesEditingService>();
            services.AddTransient<ISeriesRatingService, SeriesRatingService>();
            services.AddTransient<ISeriesSearchQueryingService, SeriesSearchQueryingService>();

            // Volume
            services.AddTransient<IVolumeAttachmentService, VolumeAttachmentService>();
            services.AddTransient<IVolumeCreationService, VolumeCreationService>();
            services.AddTransient<IVolumeDeletionService, VolumeDeletionService>();
            services.AddTransient<IVolumeDetachmentService, VolumeDetachmentService>();
            services.AddTransient<IVolumeDetailsService, VolumeDetailsService>();
            services.AddTransient<IVolumeEditingInfoService, VolumeEditingInfoService>();
            services.AddTransient<IVolumeEditingService, VolumeEditingService>();
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
                        endpoints.MapControllerRoute(
                            name: "areaDetails",
                            pattern: "{controller}/{id?}",
                            defaults: new { area = "Administration", action = "Index" },
                            constraints: new { id = @"\d+" });
                        endpoints.MapControllerRoute(
                            name: "details",
                            pattern: "{controller}/{id}",
                            defaults: new { action = "Index" },
                            constraints: new { id = @"\d+" });

                        endpoints.MapControllerRoute(
                            name: "areaRoute",
                            pattern: "{controller}/{action}/{id?}",
                            defaults: new { area = "Administration" },
                            constraints: new { id = @"\d+" });
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
