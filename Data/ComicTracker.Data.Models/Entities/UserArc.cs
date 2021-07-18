namespace ComicTracker.Data.Models.Entities
{
    using ComicTracker.Data.Models.Entities.BaseModels;

    public class UserArc : BaseUserEntityModel
    {
        public int ArcId { get; set; }

        public virtual Arc Arc { get; set; }
    }
}
