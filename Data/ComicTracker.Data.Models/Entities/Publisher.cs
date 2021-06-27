namespace ComicTracker.Data.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Publisher
    {
        public Publisher()
        {
            this.Series = new HashSet<Series>();
            this.Arcs = new HashSet<Arc>();
            this.Volumes = new HashSet<Volume>();
            this.PublishersVolumes = new List<PublisherVolume>();
            this.Issues = new HashSet<Issue>();
            this.PublishersIssues = new List<PublisherIssue>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime FoundingDate { get; set; }

        // It makes no sense for a publisher's country of origin to be unknown.
        public int NationalityId { get; set; }

        [Required]
        public Nationality Nationality { get; set; }

        public ICollection<Series> Series { get; set; }

        public ICollection<Arc> Arcs { get; set; }

        public ICollection<Volume> Volumes { get; set; }

        public List<PublisherVolume> PublishersVolumes { get; set; }

        public ICollection<Issue> Issues { get; set; }

        public List<PublisherIssue> PublishersIssues { get; set; }
    }
}
