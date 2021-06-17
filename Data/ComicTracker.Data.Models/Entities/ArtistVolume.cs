using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArtistVolume
    {
        public int ArtistsId { get; set; }
        public int VolumesId { get; set; }

        public virtual Artist Artists { get; set; }
        public virtual Volume Volumes { get; set; }
    }
}
