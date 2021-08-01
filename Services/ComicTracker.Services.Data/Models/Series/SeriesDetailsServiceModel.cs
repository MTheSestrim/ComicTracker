namespace ComicTracker.Services.Data.Models.Series
{
    using System.Collections.Generic;

    using ComicTracker.Services.Data.Models.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    public class SeriesDetailsServiceModel : IEntityServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public bool Ongoing { get; set; }

        public string TotalScore { get; set; }

        public string UserScore { get; set; }

        public string Description { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> Issues { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> Volumes { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> Arcs { get; set; }

        public IReadOnlyCollection<PublisherLinkingModel> Publishers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Writers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Artists { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Characters { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
