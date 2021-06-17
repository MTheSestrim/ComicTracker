#nullable disable

namespace ComicTracker.Web.Models
{
    using System.Collections.Generic;

    public partial class Series
    {
        public Series()
        {
            this.Arcs = new HashSet<Arc>();
            this.ArtistSeries = new HashSet<ArtistSeries>();
            this.CharactersSeries = new HashSet<CharactersSeries>();
            this.GenreSeries = new HashSet<GenreSeries>();
            this.Issues = new HashSet<Issue>();
            this.PublisherSeries = new HashSet<PublisherSeries>();
            this.SeriesWriters = new HashSet<SeriesWriter>();
            this.Volumes = new HashSet<Volume>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string CoverPath { get; set; }

        public bool Ongoing { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Arc> Arcs { get; set; }

        public virtual ICollection<ArtistSeries> ArtistSeries { get; set; }

        public virtual ICollection<CharactersSeries> CharactersSeries { get; set; }

        public virtual ICollection<GenreSeries> GenreSeries { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }

        public virtual ICollection<PublisherSeries> PublisherSeries { get; set; }

        public virtual ICollection<SeriesWriter> SeriesWriters { get; set; }

        public virtual ICollection<Volume> Volumes { get; set; }
    }
}
