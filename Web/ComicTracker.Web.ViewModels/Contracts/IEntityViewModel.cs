namespace ComicTracker.Web.ViewModels.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Web.ViewModels.Series;

    public interface IEntityViewModel
    {
        // Id is necessary for query comparisons; Need to know identity of current series
        public int Id { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public string Description { get; set; }

        public IReadOnlyCollection<PublisherLinkingModel> Publishers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Writers { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Artists { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Characters { get; set; }

        public IReadOnlyCollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
