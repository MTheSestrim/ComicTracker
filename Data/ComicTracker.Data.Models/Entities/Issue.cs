﻿namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Issue
    {
        public Issue()
        {
            this.Publishers = new HashSet<Publisher>();
            this.PublishersIssues = new List<PublisherIssue>();
            this.Writers = new HashSet<Writer>();
            this.Artists = new HashSet<Artist>();
            this.Characters = new HashSet<Character>();
            this.CharactersIssues = new List<CharacterIssue>();
            this.Genres = new HashSet<Genre>();
        }

        [Key]
        public int Id { get; set; }

        // Number of issue
        public int Number { get; set; }

        // Issue might not have a specific title.
        [MaxLength(150)]
        public string Title { get; set; }

        // Optional description
        public string Description { get; set; }

        // Cover of issue
        public string CoverPath { get; set; }

        // An issue belongs to only one series.
        public int SeriesId { get; set; }

        public Series Series { get; set; }

        // An issue can belong to an arc. Or not.
        public int? ArcId { get; set; }

        public Arc Arc { get; set; }

        // An issue can belong to a volume. Or not.
        public int? VolumeId { get; set; }

        public Volume Volume { get; set; }

        // Issues can have more than one publisher, like when they get a translation.
        public ICollection<Publisher> Publishers { get; set; }

        public List<PublisherIssue> PublishersIssues { get; set; }

        // Each issue can have several writers
        public ICollection<Writer> Writers { get; set; }

        // Each issue can have several artists
        public ICollection<Artist> Artists { get; set; }

        // Each issue has many characters
        public ICollection<Character> Characters { get; set; }

        public List<CharacterIssue> CharactersIssues { get; set; }

        // Each issue can have multiple genres
        public ICollection<Genre> Genres { get; set; }
    }
}
