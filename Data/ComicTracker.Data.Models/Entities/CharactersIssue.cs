using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class CharactersIssue
    {
        public int CharacterId { get; set; }
        public int IssueId { get; set; }
        public bool IsMainCharacter { get; set; }

        public virtual Character Character { get; set; }
        public virtual Issue Issue { get; set; }
    }
}
