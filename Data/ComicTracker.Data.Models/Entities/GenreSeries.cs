using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class GenreSeries
    {
        public int GenresId { get; set; }
        public int SeriesId { get; set; }

        public virtual Genre Genres { get; set; }
        public virtual Series Series { get; set; }
    }
}
