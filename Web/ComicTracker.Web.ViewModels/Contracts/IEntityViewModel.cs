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

        public ICollection<PublisherLinkingModel> Publishers { get; set; }

        public ICollection<NameOnlyLinkingModel> Writers { get; set; }

        public ICollection<NameOnlyLinkingModel> Artists { get; set; }

        public ICollection<NameOnlyLinkingModel> Characters { get; set; }

        public ICollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
