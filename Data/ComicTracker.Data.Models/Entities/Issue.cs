#nullable disable

namespace ComicTracker.Web.Models
{
    using System.Collections.Generic;

    public partial class Issue
    {
        public Issue()
        {
            this.ArtistIssues = new HashSet<ArtistIssue>();
            this.CharactersIssues = new HashSet<CharactersIssue>();
            this.GenreIssues = new HashSet<GenreIssue>();
            this.IssueWriters = new HashSet<IssueWriter>();
            this.PublishersIssues = new HashSet<PublishersIssue>();
        }

        public int Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public int SeriesId { get; set; }

        public int? ArcId { get; set; }

        public int? VolumeId { get; set; }

        public string Description { get; set; }

        public virtual Arc Arc { get; set; }

        public virtual Series Series { get; set; }

        public virtual Volume Volume { get; set; }

        public virtual ICollection<ArtistIssue> ArtistIssues { get; set; }

        public virtual ICollection<CharactersIssue> CharactersIssues { get; set; }

        public virtual ICollection<GenreIssue> GenreIssues { get; set; }

        public virtual ICollection<IssueWriter> IssueWriters { get; set; }

        public virtual ICollection<PublishersIssue> PublishersIssues { get; set; }
    }
}
