namespace ComicTracker.Web.ViewModels.Entities
{
    using System.Collections.Generic;

    using ComicTracker.Web.Infrastructure;
    using Microsoft.AspNetCore.Http;

    using static ComicTracker.Common.GlobalConstants;

    public class EditEntityInputModel
    {
        public int Id { get; init; }

        public string Description { get; init; }

        public string CoverPath { get; init; }

        [MaxFileSize(DefaultImageSizeInKB * BytesInAKilobyte)]
        public IFormFile CoverImage { get; init; }

        public IEnumerable<int> Genres { get; init; }

        public IEnumerable<KeyValuePair<string, string>> RetrievedGenres { get; set; }
    }
}
