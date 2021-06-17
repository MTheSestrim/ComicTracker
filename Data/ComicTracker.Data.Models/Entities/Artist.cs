#nullable disable

namespace ComicTracker.Web.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Artist
    {
        public Artist()
        {
            this.ArcArtists = new HashSet<ArcArtist>();
            this.ArtistIssues = new HashSet<ArtistIssue>();
            this.ArtistSeries = new HashSet<ArtistSeries>();
            this.ArtistVolumes = new HashSet<ArtistVolume>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public string Bio { get; set; }

        public int? NationalityId { get; set; }

        public virtual Nationality Nationality { get; set; }

        public virtual ICollection<ArcArtist> ArcArtists { get; set; }

        public virtual ICollection<ArtistIssue> ArtistIssues { get; set; }

        public virtual ICollection<ArtistSeries> ArtistSeries { get; set; }

        public virtual ICollection<ArtistVolume> ArtistVolumes { get; set; }
    }
}
