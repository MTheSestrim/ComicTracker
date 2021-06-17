using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class CharactersArc
    {
        public int CharacterId { get; set; }
        public int ArcId { get; set; }
        public bool IsMainCharacter { get; set; }

        public virtual Arc Arc { get; set; }
        public virtual Character Character { get; set; }
    }
}
