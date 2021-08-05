namespace ComicTracker.Services.Data.Models.Entities
{
    using System.Collections.Generic;

    // ServiceModel for creation of Volumes/Arcs/Issues since they all contain the same data for this action
    public class CreateSeriesRelatedEntityServiceModel
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
