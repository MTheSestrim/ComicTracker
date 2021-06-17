#nullable disable

namespace ComicTracker.Web.Models
{
    using System.Collections.Generic;

    public partial class Character
    {
        public Character()
        {
            this.CharactersArcs = new HashSet<CharactersArc>();
            this.CharactersIssues = new HashSet<CharactersIssue>();
            this.CharactersSeries = new HashSet<CharactersSeries>();
            this.CharactersVolumes = new HashSet<CharactersVolume>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstAppearance { get; set; }

        public string Description { get; set; }

        public virtual ICollection<CharactersArc> CharactersArcs { get; set; }

        public virtual ICollection<CharactersIssue> CharactersIssues { get; set; }

        public virtual ICollection<CharactersSeries> CharactersSeries { get; set; }

        public virtual ICollection<CharactersVolume> CharactersVolumes { get; set; }
    }
}
