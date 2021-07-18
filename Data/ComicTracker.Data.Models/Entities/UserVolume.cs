namespace ComicTracker.Data.Models.Entities
{
    using ComicTracker.Data.Models.Entities.BaseModels;

    public class UserVolume : BaseUserEntityModel
    {
        public int VolumeId { get; set; }

        public virtual Volume Volume { get; set; }
    }
}
