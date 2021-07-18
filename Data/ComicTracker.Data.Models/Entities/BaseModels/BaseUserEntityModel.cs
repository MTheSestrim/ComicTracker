namespace ComicTracker.Data.Models.Entities.BaseModels
{
    public class BaseUserEntityModel
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? Score { get; set; }
    }
}
