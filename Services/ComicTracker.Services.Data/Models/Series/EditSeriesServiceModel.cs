﻿namespace ComicTracker.Services.Data.Models.Series
{
    using System.Collections.Generic;

    public class EditSeriesServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Ongoing { get; set; }

        public string Description { get; set; }

        public IEnumerable<int> Genres { get; set; }
    }
}