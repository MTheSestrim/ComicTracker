﻿namespace ComicTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.List;

    public class ListService : IListService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public ListService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public IEnumerable<ListViewModel> GetListData() => this.seriesRepository
               .All()
               .Select(s => new ListViewModel
               {
                   Title = s.Name,
                   CoverPath = s.CoverPath,
                   Score = 10,
                   IssueCount = s.Issues.Count,
                   VolumeCount = s.Volumes.Count,
                   ArcCount = s.Arcs.Count,
               })
               .ToList();
    }
}
