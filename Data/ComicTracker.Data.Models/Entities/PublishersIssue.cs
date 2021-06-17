using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class PublishersIssue
    {
        public int PublisherId { get; set; }
        public int IssueId { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual Issue Issue { get; set; }
        public virtual Publisher Publisher { get; set; }
    }
}
