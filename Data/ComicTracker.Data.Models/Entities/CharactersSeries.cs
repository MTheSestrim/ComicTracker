using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class CharactersSeries
    {
        public int CharacterId { get; set; }
        public int SeriesId { get; set; }
        public bool IsMainCharacter { get; set; }

        public virtual Character Character { get; set; }
        public virtual Series Series { get; set; }
    }
}
