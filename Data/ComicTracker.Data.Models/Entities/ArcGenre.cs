using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArcGenre
    {
        public int ArcsId { get; set; }
        public int GenresId { get; set; }

        public virtual Arc Arcs { get; set; }
        public virtual Genre Genres { get; set; }
    }
}
