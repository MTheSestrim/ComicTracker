﻿namespace ComicTracker.Web.ViewModels.Series
{
    using System.Collections.Generic;

    using ComicTracker.Web.ViewModels.Contracts;
    using ComicTracker.Web.ViewModels.Entities;

    public class SeriesDetailsViewModel : IEntityViewModel
    {
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
