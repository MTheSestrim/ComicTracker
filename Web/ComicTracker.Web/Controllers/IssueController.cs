namespace ComicTracker.Web.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class IssueController : BaseController
    {
        private readonly IIssueDetailsService issueDetailsService;

        public IssueController(IIssueDetailsService issueDetailsService)
        {
            this.issueDetailsService = issueDetailsService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var currentIssue = await this.issueDetailsService.GetIssueAsync(id);

            if (currentIssue == null)
            {
                return this.NotFound(currentIssue);
            }

            return this.View(currentIssue);
        }
    }
}
