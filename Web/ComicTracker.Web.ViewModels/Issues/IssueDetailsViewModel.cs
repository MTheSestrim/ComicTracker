namespace ComicTracker.Web.ViewModels.Issues
{
    using System.Collections.Generic;

    using ComicTracker.Web.ViewModels.Contracts;
    using ComicTracker.Web.ViewModels.Series;

    public class IssueDetailsViewModel : IEntityViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public string Description { get; set; }

        public string SeriesTitle { get; set; }

        public int SeriesId { get; set; }

        public string VolumeTitle { get; set; }

        public int? VolumeId { get; set; }

        public string ArcTitle { get; set; }

        public int? ArcId { get; set; }

        public ICollection<PublisherLinkingModel> Publishers { get; set; }

        public ICollection<NameOnlyLinkingModel> Writers { get; set; }

        public ICollection<NameOnlyLinkingModel> Artists { get; set; }

        public ICollection<NameOnlyLinkingModel> Characters { get; set; }

        public ICollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
