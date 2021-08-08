namespace ComicTracker.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Authorization;
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
