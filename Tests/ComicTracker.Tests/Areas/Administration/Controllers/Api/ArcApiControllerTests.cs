namespace ComicTracker.Tests.Areas.Administration.Controllers.Api
{
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Areas.Administration.Controllers.Api;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class ArcApiControllerTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 5)]
        [InlineData(3, 1500)]
        public void CreateArcsShouldCreateGivenNumberOfArcsAndAttachThemToSeries(int seriesId, int numberOfEntities)
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(SeriesWithId(seriesId)))
                // Act
                .Calling(c => c.CreateArcs(
                    new TemplateCreateApiRequestModel { SeriesId = seriesId, NumberOfEntities = numberOfEntities }))
                // Assert
                .ShouldReturn()
                .Result(numberOfEntities);
    }
}
