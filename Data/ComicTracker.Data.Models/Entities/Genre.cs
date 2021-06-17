#nullable disable

namespace ComicTracker.Web.Models
{
    using System.Collections.Generic;

    public partial class Genre
    {
        public Genre()
        {
            this.ArcGenres = new HashSet<ArcGenre>();
            this.GenreIssues = new HashSet<GenreIssue>();
            this.GenreSeries = new HashSet<GenreSeries>();
            this.GenreVolumes = new HashSet<GenreVolume>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ArcGenre> ArcGenres { get; set; }

        public virtual ICollection<GenreIssue> GenreIssues { get; set; }

        public virtual ICollection<GenreSeries> GenreSeries { get; set; }

        public virtual ICollection<GenreVolume> GenreVolumes { get; set; }
    }
}
