// ReSharper disable VirtualMemberCallInConstructor
namespace ComicTracker.Data.Models
{
    using System;
    using System.Collections.Generic;

    using ComicTracker.Data.Common.Models;
    using ComicTracker.Data.Models.Entities;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.UsersSeries = new HashSet<UserSeries>();
            this.UsersArcs = new HashSet<UserArc>();
            this.UsersVolumes = new HashSet<UserVolume>();
            this.UsersIssues = new HashSet<UserIssue>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        // Each user can rate each series.
        public virtual ICollection<UserSeries> UsersSeries { get; set; }

        // Each user can rate each arc.
        public virtual ICollection<UserArc> UsersArcs { get; set; }

        // Each user can rate each volume.
        public virtual ICollection<UserVolume> UsersVolumes { get; set; }

        // Each user can score each issue.
        public virtual ICollection<UserIssue> UsersIssues { get; set; }
    }
}
