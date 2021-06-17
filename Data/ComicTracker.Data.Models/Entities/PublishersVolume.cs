using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class PublishersVolume
    {
        public int PublisherId { get; set; }
        public int VolumeId { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual Volume Volume { get; set; }
    }
}
