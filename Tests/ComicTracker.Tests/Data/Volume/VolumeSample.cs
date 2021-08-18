namespace ComicTracker.Tests.Data.Volume
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Models.Entities;

    public class VolumeSample
    {
        public static Volume VolumeWithId(int id) => new() { Id = id };

        public static Volume SREVolumeWithIdAndNumber(int id, int number, int seriesId) => new()
        {
            Id = id,
            Number = number,
            SeriesId = seriesId,
        };

        public static Volume SREVolumeWithIdAndNumberAndArcs(
            int id,
            int number,
            int seriesId,
            int minRange,
            int maxRange)
        {
            var arcsVolumes = Enumerable.Range(minRange, maxRange - minRange + 1)
                .Select(i => new ArcVolume
                {
                    ArcId = i,
                    VolumeId = id,
                }).ToList();

            return new Volume
            {
                Id = id,
                Number = number,
                SeriesId = seriesId,
                ArcsVolumes = arcsVolumes,
            };
        }

        public static Volume SREVolumeWithIdAndNumberAndIssues(
            int id,
            int number,
            int seriesId,
            int minRange,
            int maxRange)
        {
            var issues = Enumerable.Range(minRange, maxRange - minRange + 1)
                .Select(i => new Issue
                {
                    Id = i,
                    Number = i,
                    VolumeId = id,
                    SeriesId = seriesId,
                }).ToList();

            return new Volume
            {
                Id = id,
                Number = number,
                SeriesId = seriesId,
                Issues = issues,
            };
        }

        public static Volume VolumeWithIdAndScore(int id, int? score, string userId)
        {
            var volume = new Volume() { Id = id };
            volume.UsersVolumes.Add(new UserVolume { Score = score, UserId = userId });

            return volume;
        }

        public static IEnumerable<Volume> TenSREVolumesWithIdsAndNumbers(int seriesId) => Enumerable.Range(1, 10)
            .Select(i => new Volume
            {
                Id = i,
                SeriesId = seriesId,
                Number = i,
            });
    }
}
