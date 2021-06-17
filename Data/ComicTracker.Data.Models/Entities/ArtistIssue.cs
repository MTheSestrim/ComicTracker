using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class ArtistIssue
    {
        public int ArtistsId { get; set; }
        public int IssuesId { get; set; }

        public virtual Artist Artists { get; set; }
        public virtual Issue Issues { get; set; }
    }
}
