namespace ComicTracker.Web.ViewModels.Series
{
    using System.Collections.Generic;

    public class SeriesDetailsViewModel
    {
        // Id is necessary for query comparisons; Need to know identity of current series
        public int Id { get; set; }

        public string Title { get; set; }

        public string CoverPath { get; set; }

        public bool Ongoing { get; set; }

        public string Description { get; set; }

        public ICollection<EntityLinkingModel> Issues { get; set; }

        public ICollection<EntityLinkingModel> Volumes { get; set; }

        public ICollection<EntityLinkingModel> Arcs { get; set; }

        public ICollection<PublisherLinkingModel> Publishers { get; set; }

        public ICollection<NameOnlyLinkingModel> Writers { get; set; }

        public ICollection<NameOnlyLinkingModel> Artists { get; set; }

        public ICollection<NameOnlyLinkingModel> Characters { get; set; }

        public ICollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
