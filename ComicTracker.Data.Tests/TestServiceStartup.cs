namespace ComicTracker.Services.Tests
{
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Services.Tests.Services.Arc;
    using ComicTracker.Services.Tests.Services.Issue;
    using ComicTracker.Services.Tests.Services.List;
    using ComicTracker.Services.Tests.Services.Series;
    using ComicTracker.Services.Tests.Services.Volume;

    using ComicTracker.Web;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MyTested.AspNetCore.Mvc;

    public class TestServiceStartup : Startup
    {
        public TestServiceStartup(IConfiguration configuration) : base(configuration)
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

            // List
            services.Replace<IListService, MockListService>(ServiceLifetime.Transient);

            // Series
            services.Replace<ISeriesDetailsService, MockSeriesDetailsService>(ServiceLifetime.Transient);
            services.Replace<ISeriesSearchQueryingService, 
                MockSeriesSearchQueryingService>(ServiceLifetime.Transient);

            // Volume
            services.Replace<IVolumeDetailsService, MockVolumeDetailsService>(ServiceLifetime.Transient);
        }
    }
}
