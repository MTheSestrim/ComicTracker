namespace ComicTracker.Web.ViewModels.Issues
{
    using System.Collections.Generic;

    using ComicTracker.Web.ViewModels.Contracts;
    using ComicTracker.Web.ViewModels.Series;

    public class IssueDetailsViewModel : IEntityViewModel, ISeriesRelatedViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public string Description { get; set; }

        public int SeriesId { get; set; }

        public string SeriesTitle { get; set; }

        public int? VolumeId { get; set; }

        public string VolumeTitle { get; set; }

        public int? VolumeNumber { get; set; }

        public int? ArcId { get; set; }

        public int? ArcNumber { get; set; }

        public string ArcTitle { get; set; }

        public IReadOnlyCollection<PublisherLinkingModel> Publishers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Writers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Artists { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Characters { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
