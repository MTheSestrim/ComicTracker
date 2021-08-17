namespace ComicTracker.Web.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Issue")]
    public class IssueApiController : BaseController
    {
        private readonly IIssueRatingService issueRatingService;
        private readonly IListIssueService listIssueService;
        private readonly IMemoryCache cache;

        public IssueApiController(
            IIssueRatingService issueRatingService,
            IListIssueService listIssueService,
            IMemoryCache cache)
        {
            this.issueRatingService = issueRatingService;
            this.listIssueService = listIssueService;
            this.cache = cache;
        }

        [HttpPut]
        [Route("Score")]
        public async Task<ActionResult<int>> ScoreIssue(RateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveIssueDetails(model.Id);

            return await this.issueRatingService.RateIssue(this.User.GetId(), model);
        }

        [HttpPut]
        [Route("AddToList/{id}")]
        public async Task<ActionResult> AddIssueToList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveIssueDetails(id);

            try
            {
                await this.listIssueService.AddIssueToList(this.User.GetId(), id);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("RemoveFromList/{id}")]
        public async Task<ActionResult> RemoveIssueFromList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveIssueDetails(id);

            try
            {
                await this.listIssueService.RemoveIssueFromList(this.User.GetId(), id);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
