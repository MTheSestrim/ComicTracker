﻿namespace ComicTracker.Services.Data.Models.List
{
    public class ListServiceModel
    {
        public string Title { get; set; }

        public string CoverPath { get; set; }

        public int? Score { get; set; }

        public int IssueCount { get; set; }

        public int VolumeCount { get; set; }

        public int ArcCount { get; set; }
    }
}