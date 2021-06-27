namespace ComicTracker.Data.Models.Entities
{
    // Entity exists to configure cascade deletion
    public class ArcVolume
    {
        public int ArcId { get; set; }

        public Arc Arc { get; set; }

        public int VolumeId { get; set; }

        public Volume Volume { get; set; }
    }
}
