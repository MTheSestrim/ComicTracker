namespace ComicTracker.Web.ViewModels.Entities
{
    using System.ComponentModel.DataAnnotations;

    using ComicTracker.Web.ViewModels.Entities.Contracts;

    using static ComicTracker.Common.GlobalConstants;

    public class EditSeriesRelatedEntityInputModel : EditEntityInputModel, IBaseSeriesRelatedEntityInputModel
    {

        // Error must be a constant expression, therefore string interpolation cannot be used in a convenient manner.
        [MaxLength(DefaultEntityTitleLength, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public new string Title { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number cannot be a negative value.")]
        public int Number { get; set; }

        public int SeriesId { get; set; }
    }
}
