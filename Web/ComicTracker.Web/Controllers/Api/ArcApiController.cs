namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/Arc")]
    public class ArcApiController : ControllerBase
    {
        private readonly IArcRatingService arcRatingService;
        private readonly IArcTemplateCreationService arcTemplateCreationService;

        public ArcApiController(
            IArcRatingService arcRatingService,
            IArcTemplateCreationService arcTemplateCreationService)
        {
            this.arcRatingService = arcRatingService;
            this.arcTemplateCreationService = arcTemplateCreationService;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreArc(RateApiRequestModel model)
            => await this.arcRatingService.RateArc(this.User.GetId(), model);

        [HttpPost]
        public ActionResult<int> CreateIssues(TemplateCreateApiRequestModel model)
        {
            return this.arcTemplateCreationService.CreateTemplateArcs(model);
        }
    }
}
