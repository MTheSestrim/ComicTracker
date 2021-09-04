namespace ComicTracker.Web.ViewModels.Error
{
    public class DefaultErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}
