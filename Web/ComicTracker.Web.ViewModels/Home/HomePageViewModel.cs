namespace ComicTracker.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class HomePageViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;

        public int TotalSeriesCount { get; set; }

        public int CurrentPage { get; set; } = 1;

        public IList<HomeSeriesViewModel> Series { get; set; }
    }
}
