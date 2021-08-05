namespace ComicTracker.Services.Data.Volume.Models
{
    using System.Collections.Generic;

    public class CreateVolumeServiceModel
    {
        public string Title { get; init; }

        public int Number { get; init; }

        public string Description { get; init; }

        public string CoverPath { get; init; }

        public byte[] CoverImage { get; init; }

        public int SeriesId { get; init; }

        public IEnumerable<int> Genres { get; init; }

        public IEnumerable<KeyValuePair<string, string>> RetrievedGenres { get; init; }
    }
}
