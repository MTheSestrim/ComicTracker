namespace ComicTracker.Services.Data.Arc.Models
{
    using System.Collections.Generic;

    using ComicTracker.Services.Data.Models.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    public class ArcDetailsServiceModel : IEntityServiceModel, ISeriesRelatedServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public string Description { get; set; }

        public int Number { get; set; }

        public bool IsInList { get; set; }

        public string TotalScore { get; set; }

        public string UserScore { get; set; }

        public int SeriesId { get; set; }

        public string SeriesTitle { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> Issues { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> Volumes { get; set; }

        public IReadOnlyCollection<PublisherLinkingModel> Publishers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Writers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Artists { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Characters { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
