namespace ComicTracker.Web.ViewModels.Entities.Contracts
{
    public interface IBaseSeriesRelatedEntityInputModel
    {
        public int Number { get; set; }

        public int SeriesId { get; set; }
    }
}
