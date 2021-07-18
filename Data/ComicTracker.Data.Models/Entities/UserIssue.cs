namespace ComicTracker.Data.Models.Entities
{
    using ComicTracker.Data.Models.Entities.BaseModels;

    public class UserIssue : BaseUserEntityModel
    {
        public int IssueId { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
