namespace ComicTracker.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;

    using ComicTracker.Common.Enums;
    using ComicTracker.Services.Data.Models.Home;

    using static ComicTracker.Common.HomeConstants;

    public class HomePageViewModel
    {
        public int CurrentPage { get; set; } = 1;

        public string SearchTerm { get; set; } = string.Empty;

        public Sorting Sorting { get; set; } = Sorting.Name;

        public int TotalSeriesCount { get; set; }

        public IList<HomeSeriesServiceModel> Series { get; set; }

        public double MaxPageCount => Math.Ceiling((double)this.TotalSeriesCount / SeriesPerPage);
    }
}
