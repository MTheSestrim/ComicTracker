namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Data.Common.Models;

    public class Character : BaseDeletableModel<int>
    {
        public Character()
        {
            this.Series = new HashSet<Series>();
            this.CharactersSeries = new List<CharacterSeries>();
            this.Arcs = new HashSet<Arc>();
            this.CharactersArcs = new List<CharacterArc>();
            this.Volumes = new HashSet<Volume>();
            this.CharactersVolumes = new List<CharacterVolume>();
            this.Issues = new HashSet<Issue>();
            this.CharactersIssues = new List<CharacterIssue>();
        }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstAppearance { get; set; }

        public string Description { get; set; }

        // One character can appear in several series.
        public ICollection<Series> Series { get; set; }

        public List<CharacterSeries> CharactersSeries { get; set; }

        // One character can appear in several arcs.
        public ICollection<Arc> Arcs { get; set; }

        public List<CharacterArc> CharactersArcs { get; set; }

        // One character can appear in several volumes.
        public ICollection<Volume> Volumes { get; set; }

        public List<CharacterVolume> CharactersVolumes { get; set; }

        // One character can appear in several issues.
        public ICollection<Issue> Issues { get; set; }

        public List<CharacterIssue> CharactersIssues { get; set; }
    }
}
