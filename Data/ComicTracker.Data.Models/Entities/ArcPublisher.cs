using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArcPublisher
    {
        public int ArcsId { get; set; }
        public int PublishersId { get; set; }

        public virtual Arc Arcs { get; set; }
        public virtual Publisher Publishers { get; set; }
    }
}
