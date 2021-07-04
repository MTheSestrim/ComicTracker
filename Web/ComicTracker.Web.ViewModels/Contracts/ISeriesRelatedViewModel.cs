namespace ComicTracker.Web.ViewModels.Contracts
{
    public interface ISeriesRelatedViewModel
    {
        public int Number { get; set; }

        public int SeriesId { get; set; }

        public string SeriesTitle { get; set; }
    }
}
