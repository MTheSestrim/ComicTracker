namespace ComicTracker.Web.Models
{
    using System.Collections.Generic;

    public partial class Arc
    {
        public Arc()
        {
            this.ArcArtists = new HashSet<ArcArtist>();

            this.ArcGenres = new HashSet<ArcGenre>();

            this.ArcPublishers = new HashSet<ArcPublisher>();

            this.ArcVolumes = new HashSet<ArcVolume>();

            this.ArcWriters = new HashSet<ArcWriter>();

            this.CharactersArcs = new HashSet<CharactersArc>();

            this.Issues = new HashSet<Issue>();
        }

        public int Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public int SeriesId { get; set; }

        public string Description { get; set; }

        public virtual Series Series { get; set; }

        public virtual ICollection<ArcArtist> ArcArtists { get; set; }

        public virtual ICollection<ArcGenre> ArcGenres { get; set; }

        public virtual ICollection<ArcPublisher> ArcPublishers { get; set; }

        public virtual ICollection<ArcVolume> ArcVolumes { get; set; }

        public virtual ICollection<ArcWriter> ArcWriters { get; set; }

        public virtual ICollection<CharactersArc> CharactersArcs { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }
    }
}
