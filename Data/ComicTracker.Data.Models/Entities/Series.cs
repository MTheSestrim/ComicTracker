namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Data.Common.Models;

    using static ComicTracker.Common.SeriesConstants;

    public class Series : BaseDeletableModel<int>
    {
        public Series()
        {
            this.Issues = new HashSet<Issue>();
            this.Volumes = new HashSet<Volume>();
            this.Arcs = new HashSet<Arc>();
            this.Publishers = new HashSet<Publisher>();
            this.Writers = new HashSet<Writer>();
            this.Artists = new HashSet<Artist>();
            this.Characters = new HashSet<Character>();
            this.CharactersSeries = new List<CharacterSeries>();
            this.Genres = new HashSet<Genre>();
            this.UsersSeries = new HashSet<UserSeries>();
        }

        [Required]
        [MaxLength(DefaultSeriesNameMaxLength)]
        public string Title { get; set; }

        // Optional description.
        public string Description { get; set; }

        // Image representing series
        public string CoverPath { get; set; }

        public bool Ongoing { get; set; }

        public ICollection<Issue> Issues { get; set; }

        public ICollection<Volume> Volumes { get; set; }

        // Most comic book series are structured in arcs,
        // so this is another way for a user to track their progress.
        // Should be optional.
        public ICollection<Arc> Arcs { get; set; }

        // Series can have more than one publisher, like when they get a translation.
        public ICollection<Publisher> Publishers { get; set; }

        // Each series can have several writers
        public ICollection<Writer> Writers { get; set; }

        // Each series can have several artists
        public ICollection<Artist> Artists { get; set; }

        // Each series has many characters
        public ICollection<Character> Characters { get; set; }

        public List<CharacterSeries> CharactersSeries { get; set; }

        // Each series can have multiple genres
        public ICollection<Genre> Genres { get; set; }

        // Each user can rate each series.
        public ICollection<UserSeries> UsersSeries { get; set; }
    }
}
