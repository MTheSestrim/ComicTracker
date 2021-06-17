using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class SeriesWriter
    {
        public int SeriesId { get; set; }
        public int WritersId { get; set; }

        public virtual Series Series { get; set; }
        public virtual Writer Writers { get; set; }
    }
}
