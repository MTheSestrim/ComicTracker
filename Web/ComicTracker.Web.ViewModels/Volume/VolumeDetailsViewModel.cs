﻿namespace ComicTracker.Web.ViewModels.Volume
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

        public ICollection<EntityLinkingModel> Issues { get; set; }

        public ICollection<EntityLinkingModel> Arcs { get; set; }

        public ICollection<PublisherLinkingModel> Publishers { get; set; }

        public ICollection<NameOnlyLinkingModel> Writers { get; set; }

        public ICollection<NameOnlyLinkingModel> Artists { get; set; }

        public ICollection<NameOnlyLinkingModel> Characters { get; set; }

        public ICollection<NameOnlyLinkingModel> Genres { get; set; }
    }
}
