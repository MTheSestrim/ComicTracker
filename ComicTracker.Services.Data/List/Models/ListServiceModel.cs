namespace ComicTracker.Services.Data.List.Models
{
    public class ListServiceModel
    {
        public string Title { get; set; }

        public string CoverPath { get; set; }

        public int? Score { get; set; }

        public int IssuesCount { get; set; }

        public int VolumesCount { get; set; }

        public int ArcsCount { get; set; }
    }
}
