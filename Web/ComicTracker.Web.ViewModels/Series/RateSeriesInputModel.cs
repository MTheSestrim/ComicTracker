namespace ComicTracker.Web.ViewModels.Series
{
    using System.ComponentModel.DataAnnotations;

    public class RateSeriesInputModel
    {
        public int SeriesId { get; set; }

        [Range(0, 10, ErrorMessage = "Score must be an integer between 0 and 10")]
        public int Score { get; set; }
    }
}
