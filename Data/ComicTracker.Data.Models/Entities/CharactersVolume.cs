using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class CharactersVolume
    {
        public int CharacterId { get; set; }
        public int VolumeId { get; set; }
        public bool IsMainCharacter { get; set; }

        public virtual Character Character { get; set; }
        public virtual Volume Volume { get; set; }
    }
}
