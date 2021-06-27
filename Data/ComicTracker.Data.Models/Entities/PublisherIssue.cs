namespace ComicTracker.Data.Models.Entities
{
    using System;

    public class PublisherIssue
    {
        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public int IssueId { get; set; }

        public Issue Issue { get; set; }

        // Different publishers (like when a work is translated) are likely to release an issue at different dates
        public DateTime? ReleaseDate { get; set; }
    }
}
