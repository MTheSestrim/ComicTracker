using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArcVolume
    {
        public int ArcId { get; set; }
        public int VolumeId { get; set; }

        public virtual Arc Arc { get; set; }
        public virtual Volume Volume { get; set; }
    }
}
