using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArtistSeries
    {
        public int ArtistsId { get; set; }
        public int SeriesId { get; set; }

        public virtual Artist Artists { get; set; }
        public virtual Series Series { get; set; }
    }
}
