namespace ComicTracker.Services.Data.Models.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IEntityServiceModel
    {
        // Id is necessary for query comparisons; Need to know identity of current series
        public int Id { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public string TotalScore { get; set; }

        public string UserScore { get; set; }

        public string Description { get; set; }

        public IReadOnlyCollection<PublisherLinkingModel> Publishers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Writers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Artists { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Characters { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
