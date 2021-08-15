namespace ComicTracker.Services.Data.Genre.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateGenreServiceModel
    {
        [Required]
        public string Name { get; init; }
    }
}
