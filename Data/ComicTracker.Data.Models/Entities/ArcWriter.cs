using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArcWriter
    {
        public int ArcsId { get; set; }
        public int WritersId { get; set; }

        public virtual Arc Arcs { get; set; }
        public virtual Writer Writers { get; set; }
    }
}
