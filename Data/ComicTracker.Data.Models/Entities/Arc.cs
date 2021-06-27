namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Arc
    {
        public Arc()
        {
            this.Issues = new HashSet<Issue>();
            this.Volumes = new HashSet<Volume>();
            this.ArcsVolumes = new List<ArcVolume>();
            this.Publishers = new HashSet<Publisher>();
            this.Writers = new HashSet<Writer>();
            this.Artists = new HashSet<Artist>();
            this.Characters = new HashSet<Character>();
            this.CharactersArcs = new List<CharacterArc>();
            this.Genres = new HashSet<Genre>();
        }

        [Key]
        public int Id { get; set; }

        // Number of arc
        public int Number { get; set; }

        // Arc might not have a specific title, though this is rare.
        [MaxLength(150)]
        public string Title { get; set; }

        // Optional description.
        public string Description { get; set; }

        // Optional image that represents Arc
        public string CoverPath { get; set; }

        public ICollection<Issue> Issues { get; set; }

        // Arcs could extend beyond one volume.
        public ICollection<Volume> Volumes { get; set; }

        public List<ArcVolume> ArcsVolumes { get; set; }

        // An arc belongs to only one series.
        public int SeriesId { get; set; }

        public Series Series { get; set; }

        // Arcs can have more than one publisher, like when they get a translation.
        public ICollection<Publisher> Publishers { get; set; }

        // Each arc can have several writers
        public ICollection<Writer> Writers { get; set; }

        // Each arc can have several artists
        public ICollection<Artist> Artists { get; set; }

        // Each arc has many characters
        public ICollection<Character> Characters { get; set; }

        public List<CharacterArc> CharactersArcs { get; set; }

        // Each arc can have multiple genres
        public ICollection<Genre> Genres { get; set; }
    }
}
