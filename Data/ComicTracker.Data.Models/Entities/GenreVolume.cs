using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class GenreVolume
    {
        public int GenresId { get; set; }
        public int VolumesId { get; set; }

        public virtual Genre Genres { get; set; }
        public virtual Volume Volumes { get; set; }
    }
}
