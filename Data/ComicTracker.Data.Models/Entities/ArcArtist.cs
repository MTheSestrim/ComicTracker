#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArcArtist
    {
        public int ArcsId { get; set; }

        public int ArtistsId { get; set; }

        public virtual Arc Arcs { get; set; }

        public virtual Artist Artists { get; set; }
    }
}
