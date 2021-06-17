using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class PublisherSeries
    {
        public int PublishersId { get; set; }
        public int SeriesId { get; set; }

        public virtual Publisher Publishers { get; set; }
        public virtual Series Series { get; set; }
    }
}
