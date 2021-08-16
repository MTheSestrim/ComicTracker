namespace ComicTracker.Services.Data.List.Models
{
    public class ListServiceModel
    {
        public string Title { get; set; }

        public string CoverPath { get; set; }

        public int? Score { get; set; }

        public int UserIssuesCount { get; set; }

        public int UserVolumesCount { get; set; }

        public int UserArcsCount { get; set; }

        public int TotalIssuesCount { get; set; }

        public int TotalVolumesCount { get; set; }

        public int TotalArcsCount { get; set; }
    }
}
