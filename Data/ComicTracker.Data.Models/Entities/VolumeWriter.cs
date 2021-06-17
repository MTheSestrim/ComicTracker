using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class VolumeWriter
    {
        public int VolumesId { get; set; }
        public int WritersId { get; set; }

        public virtual Volume Volumes { get; set; }
        public virtual Writer Writers { get; set; }
    }
}
