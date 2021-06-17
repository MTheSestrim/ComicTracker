#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class GenreIssue
    {
        public int GenresId { get; set; }

        public int IssuesId { get; set; }

        public virtual Genre Genres { get; set; }

        public virtual Issue Issues { get; set; }
    }
}
