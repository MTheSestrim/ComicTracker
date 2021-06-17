#nullable disable

namespace ComicTracker.Web.Models
{
    using System.Collections.Generic;

    public partial class Nationality
    {
        public Nationality()
        {
            this.Artists = new HashSet<Artist>();
            this.Publishers = new HashSet<Publisher>();
            this.Writers = new HashSet<Writer>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }

        public virtual ICollection<Publisher> Publishers { get; set; }

        public virtual ICollection<Writer> Writers { get; set; }
    }
}
