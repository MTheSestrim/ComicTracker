namespace ComicTracker.Data.Models.Entities
{
    using System;

    public class PublisherVolume
    {
        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public int VolumeId { get; set; }

        public Volume Volume { get; set; }

        // Different publishers (like when a work is translated) are likely to release a volume at different dates.
        // Volumes tend to have separate release dates from the issues they contain,
        // which is why this join entity exists.
        public DateTime? ReleaseDate { get; set; }
    }
}
