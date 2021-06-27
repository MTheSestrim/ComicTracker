namespace ComicTracker.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Genre
    {
        public Genre()
        {
            this.Series = new HashSet<Series>();
            this.Arcs = new HashSet<Arc>();
            this.Volumes = new HashSet<Volume>();
            this.Issues = new HashSet<Issue>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // There are many series that belong to one genre
        public ICollection<Series> Series { get; set; }

        // There are many series that belong to one genre
        public ICollection<Arc> Arcs { get; set; }

        // There are many volumes that belong to one genre
        public ICollection<Volume> Volumes { get; set; }

        // There are many issues that belong to one genre
        public ICollection<Issue> Issues { get; set; }
    }
}
