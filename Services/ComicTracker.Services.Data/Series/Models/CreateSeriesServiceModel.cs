namespace ComicTracker.Services.Data.Series.Models
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public class CreateSeriesServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverPath { get; set; }

        public IFormFile CoverImage { get; set; }

        public bool Ongoing { get; set; }

        public IEnumerable<int> Genres { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RetrievedGenres { get; set; }
    }
}
