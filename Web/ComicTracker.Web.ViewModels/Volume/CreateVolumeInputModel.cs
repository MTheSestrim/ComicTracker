namespace ComicTracker.Web.ViewModels.Volume
{
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Web.ViewModels.Entities;

    using static ComicTracker.Common.GlobalConstants;

    public class CreateVolumeInputModel : CreateEntityInputModel
    {
        // Error must be a constant expression, therefore string interpolation cannot be used in a convenient manner.
        [MaxLength(DefaultEntityTitleLength, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public new string Title { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "Volume number cannot be a negative value.")]
        public int Number { get; set; }

        public int SeriesId { get; set; }
    }
}
