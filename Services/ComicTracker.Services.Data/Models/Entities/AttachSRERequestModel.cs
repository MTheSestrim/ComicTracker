namespace ComicTracker.Services.Data.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class AttachSRERequestModel
    {
        public int SeriesId { get; init; }

        [Required]
        public string ParentTypeName { get; set; }

        public int ParentId { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "MinRange must be a positive value.")]
        public int MinRange { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "MaxRange must be a positive value.")]
        public int MaxRange { get; init; }
    }
}
