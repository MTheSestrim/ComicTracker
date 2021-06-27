namespace ComicTracker.Data.Models.Entities
{
    public class CharacterIssue
    {
        public int CharacterId { get; set; }

        public Character Character { get; set; }

        public int IssueId { get; set; }

        public Issue Issue { get; set; }

        // Determines if character should be displayed with top priority for an issue.
        // Character can be main in one issue and secondary in another.
        public bool IsMainCharacter { get; set; }
    }
}
