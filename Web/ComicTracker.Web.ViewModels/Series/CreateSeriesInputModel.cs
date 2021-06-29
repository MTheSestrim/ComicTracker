namespace ComicTracker.Web.ViewModels.Series
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateSeriesInputModel
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverPath { get; set; }

        public bool Ongoing { get; set; }

        public IEnumerable<int> Genres { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RetrievedGenres { get; set; }
    }
}
