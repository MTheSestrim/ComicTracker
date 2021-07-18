namespace ComicTracker.Data.Models.Entities
{
    using ComicTracker.Data.Models.Entities.BaseModels;

    public class UserSeries : BaseUserEntityModel
    {
        public int SeriesId { get; set; }

        public virtual Series Series { get; set; }
    }
}
