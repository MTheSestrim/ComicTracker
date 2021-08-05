namespace ComicTracker.Web.ViewModels.Series
{
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Web.ViewModels.Entities;

    using static ComicTracker.Common.SeriesConstants;

    public class CreateSeriesInputModel : CreateEntityInputModel
    {
        // Error must be a constant expression, therefore string interpolation cannot be used in a convenient manner.
        [Required]
        [StringLength(
            DefaultSeriesNameMaxLength,
            MinimumLength = DefaultSeriesNameMinLength,
            ErrorMessage = "Title must be between 2 and 200 characters.")]
        public new string Title { get; set; }

        public bool Ongoing { get; set; }
    }
}
