namespace ComicTracker.Web.ViewModels.Series
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Http;

    using static ComicTracker.Common.GlobalConstants;
    using static ComicTracker.Common.SeriesConstants;

    public class CreateSeriesInputModel
    {
        // Error must be a constant expression, therefore string interpolation cannot be used in a convenient manner.
        [Required]
        [StringLength(
            DefaultSeriesNameMaxLength,
            MinimumLength = DefaultSeriesNameMinLength,
            ErrorMessage = "Name must be between 2 and 200 characters.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverPath { get; set; }

        [MaxFileSize(DefaultImageSizeInKB * BytesInAKilobyte)]
        public IFormFile CoverImage { get; set; }

        public bool Ongoing { get; set; }

        public IEnumerable<int> Genres { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RetrievedGenres { get; set; }
    }
}
