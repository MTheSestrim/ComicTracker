#nullable disable

namespace ComicTracker.Web.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Writer
    {
        public Writer()
        {
            this.ArcWriters = new HashSet<ArcWriter>();
            this.IssueWriters = new HashSet<IssueWriter>();
            this.SeriesWriters = new HashSet<SeriesWriter>();
            this.VolumeWriters = new HashSet<VolumeWriter>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public string Bio { get; set; }

        public int? NationalityId { get; set; }

        public virtual Nationality Nationality { get; set; }

        public virtual ICollection<ArcWriter> ArcWriters { get; set; }

        public virtual ICollection<IssueWriter> IssueWriters { get; set; }

        public virtual ICollection<SeriesWriter> SeriesWriters { get; set; }

        public virtual ICollection<VolumeWriter> VolumeWriters { get; set; }
    }
}
