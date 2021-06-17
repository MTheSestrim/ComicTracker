#nullable disable

namespace ComicTracker.Web.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Publisher
    {
        public Publisher()
        {
            this.ArcPublishers = new HashSet<ArcPublisher>();
            this.PublisherSeries = new HashSet<PublisherSeries>();
            this.PublishersIssues = new HashSet<PublishersIssue>();
            this.PublishersVolumes = new HashSet<PublishersVolume>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime FoundingDate { get; set; }

        public int NationalityId { get; set; }

        public virtual Nationality Nationality { get; set; }

        public virtual ICollection<ArcPublisher> ArcPublishers { get; set; }

        public virtual ICollection<PublisherSeries> PublisherSeries { get; set; }

        public virtual ICollection<PublishersIssue> PublishersIssues { get; set; }

        public virtual ICollection<PublishersVolume> PublishersVolumes { get; set; }
    }
}
