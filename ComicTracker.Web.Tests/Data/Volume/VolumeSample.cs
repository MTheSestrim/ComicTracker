namespace ComicTracker.Web.Tests.Data.Volume
{
    using ComicTracker.Data.Models.Entities;

    public class VolumeSample
    {
        public static Volume VolumeWithId(int id) => new() { Id = id };
    }
}
