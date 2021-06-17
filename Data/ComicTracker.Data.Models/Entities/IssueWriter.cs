using System;
using System.Collections.Generic;

#nullable disable

namespace ComicTracker.Web.Models
{
    public partial class IssueWriter
    {
        public int IssuesId { get; set; }
        public int WritersId { get; set; }

        public virtual Issue Issues { get; set; }
        public virtual Writer Writers { get; set; }
    }
}
