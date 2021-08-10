namespace ComicTracker.Web.Tests
{
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Tests.Services.Arc;
    using ComicTracker.Web.Tests.Services.Issue;
    using ComicTracker.Web.Tests.Services.List;
    using ComicTracker.Web.Tests.Services.Series;
    using ComicTracker.Web.Tests.Services.Volume;

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
            services.Replace<IArcDetailsService, MockArcDetailsService>(ServiceLifetime.Transient);
            services.Replace<IIssueDetailsService, MockIssueDetailsService>(ServiceLifetime.Transient);
            services.Replace<IListService, MockListService>(ServiceLifetime.Transient);
            services.Replace<ISeriesDetailsService, MockSeriesDetailsService>(ServiceLifetime.Transient);
            services.Replace<ISeriesSearchQueryingService, 
                MockSeriesSearchQueryingService>(ServiceLifetime.Transient);
            services.Replace<IVolumeDetailsService, MockVolumeDetailsService>(ServiceLifetime.Transient);
        }
    }
}
