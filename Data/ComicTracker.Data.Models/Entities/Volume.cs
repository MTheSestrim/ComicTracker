#nullable disable

namespace ComicTracker.Web.Models
{
    using System.Collections.Generic;

    public partial class Volume
    {
        public Volume()
        {
            this.ArcVolumes = new HashSet<ArcVolume>();
            this.ArtistVolumes = new HashSet<ArtistVolume>();
            this.CharactersVolumes = new HashSet<CharactersVolume>();
            this.GenreVolumes = new HashSet<GenreVolume>();
            this.Issues = new HashSet<Issue>();
            this.PublishersVolumes = new HashSet<PublishersVolume>();
            this.VolumeWriters = new HashSet<VolumeWriter>();
        }

        public int Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public int SeriesId { get; set; }

        public string Description { get; set; }

        public virtual Series Series { get; set; }

        public virtual ICollection<ArcVolume> ArcVolumes { get; set; }

        public virtual ICollection<ArtistVolume> ArtistVolumes { get; set; }

        public virtual ICollection<CharactersVolume> CharactersVolumes { get; set; }

        public virtual ICollection<GenreVolume> GenreVolumes { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }

        public virtual ICollection<PublishersVolume> PublishersVolumes { get; set; }

        public virtual ICollection<VolumeWriter> VolumeWriters { get; set; }
    }
}
