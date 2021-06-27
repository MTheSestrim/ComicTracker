namespace ComicTracker.Data.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Artist
    {
        public Artist()
        {
            this.Series = new HashSet<Series>();
            this.Arcs = new HashSet<Arc>();
            this.Volumes = new HashSet<Volume>();
            this.Issues = new HashSet<Issue>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public string Bio { get; set; }

        // Artists could have unknown nationality.
        public int? NationalityId { get; set; }

        public Nationality Nationality { get; set; }

        // Artists have usually worked on several series throughout their career.
        public ICollection<Series> Series { get; set; }

        // Artists have usually worked on several arcs throughout their career.
        public ICollection<Arc> Arcs { get; set; }

        // Artists have usually worked on several volumes throughout their career.
        public ICollection<Volume> Volumes { get; set; }

        // Artists have usually worked on several issues throughout their career.
        public ICollection<Issue> Issues { get; set; }
    }
}
