namespace ComicTracker.Tests.Data.Volume
{
    using ComicTracker.Data.Models.Entities;

    public class VolumeSample
    {
        public static Volume VolumeWithId(int id) => new() { Id = id };

        public static Volume VolumeWithIdAndScore(int id, int score, string userId)
        {
            var volume = new Volume() { Id = id };
            volume.UsersVolumes.Add(new UserVolume { Score = score, UserId = userId });

            return volume;
        }
    }
}
