namespace ComicTracker.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using ComicTracker.Common.Enums;

    public class HomePageViewModel
    {
        public int CurrentPage { get; set; } = 1;

        public string SearchTerm { get; set; } = string.Empty;

        public Sorting Sorting { get; set; } = Sorting.Name;

        public int TotalSeriesCount { get; set; }

        public IList<HomeSeriesViewModel> Series { get; set; }
    }
}
