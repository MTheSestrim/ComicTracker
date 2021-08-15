namespace ComicTracker.Tests
{
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Tests.Services.Arc;
    using ComicTracker.Tests.Services.Issue;
    using ComicTracker.Tests.Services.List;
    using ComicTracker.Tests.Services.Series;
    using ComicTracker.Tests.Services.Volume;
    using ComicTracker.Web;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MyTested.AspNetCore.Mvc;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {

        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            /* Mocking services is necessary as the real ones make queries to the SQL server, returning null results. This happens due to heavy usage of IQueryable. */

            // Arc
            services.Replace<IArcDetailsService, MockArcDetailsService>(ServiceLifetime.Transient);
            services.Replace<IArcRatingService, MockArcRatingService>(ServiceLifetime.Transient);
            services.Replace<IArcTemplateCreationService, 
                MockArcTemplateCreationService>(ServiceLifetime.Transient);

            // Issue
            services.Replace<IIssueDetailsService, MockIssueDetailsService>(ServiceLifetime.Transient);
            services.Replace<IIssueRatingService, MockIssueRatingService>(ServiceLifetime.Transient);
            services.Replace<IIssueTemplateCreationService, 
                MockIssueTemplateCreationService>(ServiceLifetime.Transient);

            // List
            services.Replace<IListService, MockListService>(ServiceLifetime.Transient);

            // Series
            services.Replace<ISeriesDetailsService, MockSeriesDetailsService>(ServiceLifetime.Transient);
            services.Replace<ISeriesSearchQueryingService, 
                MockSeriesSearchQueryingService>(ServiceLifetime.Transient);
            services.Replace<ISeriesRatingService, MockSeriesRatingService>(ServiceLifetime.Transient);

            // Volume
            services.Replace<IVolumeDetailsService, MockVolumeDetailsService>(ServiceLifetime.Transient);
            services.Replace<IVolumeRatingService, MockVolumeRatingService>(ServiceLifetime.Transient);
            services.Replace<IVolumeTemplateCreationService, 
                MockVolumeTemplateCreationService>(ServiceLifetime.Transient);
        }
    }
}
