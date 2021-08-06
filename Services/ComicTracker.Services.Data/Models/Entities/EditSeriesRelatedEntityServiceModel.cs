namespace ComicTracker.Services.Data.Models.Entities
{
    using System.Collections.Generic;

    public class EditSeriesRelatedEntityServiceModel
    {
        public int Id { get; init; }

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
