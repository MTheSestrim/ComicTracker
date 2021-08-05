namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    public class IssueController : BaseController
    {
        private readonly IIssueDetailsService issueDetailsService;

        public IssueController(IIssueDetailsService issueDetailsService)
        {
            this.issueDetailsService = issueDetailsService;
        }

        public IActionResult Index(int id)
        {
            var currentIssue = this.issueDetailsService.GetIssue(id, this.User.GetId());

            if (currentIssue == null)
            {
                return this.NotFound(currentIssue);
            }

            return this.View(currentIssue);
        }
    }
}
