namespace ComicTracker.Data.Models.Entities.BaseModels
{
    using ComicTracker.Data.Common.Models;
    using ComicTracker.Data.Models.Entities;

    public class BaseSeriesRelatedModel<TKey> : BaseDeletableModel<TKey>
    {
        public int Number { get; set; }

        public int SeriesId { get; set; }

        public Series Series { get; set; }
    }
}
