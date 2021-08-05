namespace ComicTracker.Web.ViewModels.Series
{
    using System.Collections.Generic;

    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Http;

    using static ComicTracker.Common.GlobalConstants;

    public class EditSeriesInputModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Ongoing { get; set; }

        public string Description { get; set; }

        public string CoverPath { get; set; }

        [MaxFileSize(DefaultImageSizeInKB * BytesInAKilobyte)]
        public IFormFile CoverImage { get; set; }

        public IEnumerable<int> Genres { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RetrievedGenres { get; set; }
    }
}
