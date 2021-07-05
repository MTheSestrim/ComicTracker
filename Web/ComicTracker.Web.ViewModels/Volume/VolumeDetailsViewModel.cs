namespace ComicTracker.Web.ViewModels.Volume
{
    using System.Collections.Generic;

    using ComicTracker.Web.ViewModels.Contracts;
    using ComicTracker.Web.ViewModels.Entities;
    using ComicTracker.Web.ViewModels.Series;

    public class VolumeDetailsViewModel : IEntityViewModel, ISeriesRelatedViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public string Description { get; set; }

        public int Number { get; set; }

        public int SeriesId { get; set; }

        public string SeriesTitle { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> Issues { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> Arcs { get; set; }

        public IReadOnlyCollection<PublisherLinkingModel> Publishers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Writers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Artists { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Characters { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
