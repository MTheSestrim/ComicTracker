﻿namespace ComicTracker.Services.Data.Models.Series
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public class EditSeriesServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Ongoing { get; set; }

        public string Description { get; set; }

        public string CoverPath { get; set; }

        public IFormFile CoverImage { get; set; }

        public IEnumerable<int> Genres { get; set; }
    }
}
