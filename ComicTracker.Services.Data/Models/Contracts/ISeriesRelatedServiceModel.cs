namespace ComicTracker.Services.Data.Models.Contracts
{
    public interface ISeriesRelatedServiceModel
    {
        public int Number { get; set; }

        public int SeriesId { get; set; }

        public string SeriesTitle { get; set; }
    }
}
