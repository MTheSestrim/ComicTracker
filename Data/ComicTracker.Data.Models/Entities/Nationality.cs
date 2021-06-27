namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Nationality
    {
        public Nationality()
        {
            this.Writers = new HashSet<Writer>();
            this.Artists = new HashSet<Artist>();
            this.Publishers = new HashSet<Publisher>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Writer> Writers { get; set; }

        public ICollection<Artist> Artists { get; set; }

        public ICollection<Publisher> Publishers { get; set; }
    }
}
