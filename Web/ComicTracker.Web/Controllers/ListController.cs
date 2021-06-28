﻿namespace ComicTracker.Web.Controllers
{
    using System.Linq;

    using ComicTracker.Data;

    using ComicTracker.Web.ViewModels.List;

    using Microsoft.AspNetCore.Mvc;

    public class ListController : BaseController
    {
        private readonly ComicTrackerDbContext context;

        public ListController(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var listData = this.context
               .Series
               .Select(s => new ListModel
               {
                   Title = s.Name,
                   CoverPath = s.CoverPath,
                   Score = 10,
                   IssueCount = s.Issues.Count,
                   VolumeCount = s.Volumes.Count,
                   ArcCount = s.Arcs.Count,
               })
               .ToList();

            return this.View(listData);
        }
    }
}
