namespace ComicTracker.Tests
{
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Tests.Mocks.Services.Arc;
    using ComicTracker.Tests.Mocks.Services.Issue;
    using ComicTracker.Tests.Mocks.Services.List;
    using ComicTracker.Tests.Mocks.Services.Series;
    using ComicTracker.Tests.Mocks.Services.Volume;
    using ComicTracker.Web;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MyTested.AspNetCore.Mvc;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
#pragma warning disable SA1100 // Do not prefix calls with base unless local implementation exists
            base.ConfigureServices(services);
#pragma warning restore SA1100 // Do not prefix calls with base unless local implementation exists

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
            services.Replace<IListArcService, MockListArcService>(ServiceLifetime.Transient);
            services.Replace<IListDataService, MockListDataService>(ServiceLifetime.Transient);
            services.Replace<IListIssueService, MockListIssueService>(ServiceLifetime.Transient);
            services.Replace<IListSeriesService, MockListSeriesService>(ServiceLifetime.Transient);
            services.Replace<IListVolumeService, MockListVolumeService>(ServiceLifetime.Transient);

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
