namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Data.Models.Entities.BaseModels;

    using static ComicTracker.Common.GlobalConstants;

    public class Volume : BaseSeriesRelatedModel<int>
    {
        public Volume()
        {
            this.Arcs = new HashSet<Arc>();
            this.ArcsVolumes = new List<ArcVolume>();
            this.Issues = new HashSet<Issue>();
            this.Publishers = new HashSet<Publisher>();
            this.PublishersVolumes = new List<PublisherVolume>();
            this.Writers = new HashSet<Writer>();
            this.Artists = new HashSet<Artist>();
            this.Characters = new HashSet<Character>();
            this.CharactersVolumes = new List<CharacterVolume>();
            this.Genres = new HashSet<Genre>();
            this.UsersVolumes = new HashSet<UserVolume>();
        }

        // Volume might not have a specific title.
        [MaxLength(DefaultEntityTitleLength)]
        public string Title { get; set; }

        // Optional description.
        public string Description { get; set; }

        // Cover of volume
        public string CoverPath { get; set; }

        // Volumes could potentially contain more than one arc.
        public ICollection<Arc> Arcs { get; set; }

        public List<ArcVolume> ArcsVolumes { get; set; }

        public ICollection<Issue> Issues { get; set; }

        // Volumes can have more than one publisher, like when they get a translation.
        public ICollection<Publisher> Publishers { get; set; }

        public List<PublisherVolume> PublishersVolumes { get; set; }

        // Each volume can have several writers
        public ICollection<Writer> Writers { get; set; }

        // Each volume can have several artists
        public ICollection<Artist> Artists { get; set; }

        // Each volume has many characters
        public ICollection<Character> Characters { get; set; }

        public List<CharacterVolume> CharactersVolumes { get; set; }

        // Each volume can have multiple genres
        public ICollection<Genre> Genres { get; set; }

        // Each user can rate each volume.
        public ICollection<UserVolume> UsersVolumes { get; set; }
    }
}
