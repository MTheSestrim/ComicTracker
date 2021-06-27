namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Data.Common.Models;

    public class Nationality : BaseDeletableModel<int>
    {
        public Nationality()
        {
            this.Writers = new HashSet<Writer>();
            this.Artists = new HashSet<Artist>();
            this.Publishers = new HashSet<Publisher>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Writer> Writers { get; set; }

        public ICollection<Artist> Artists { get; set; }

        public ICollection<Publisher> Publishers { get; set; }
    }
}
