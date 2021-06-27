namespace ComicTracker.Data.Models.Entities
{
    public class CharacterArc
    {
        public int CharacterId { get; set; }

        public Character Character { get; set; }

        public int ArcId { get; set; }

        public Arc Arc { get; set; }

        // Determines if character should be displayed with top priority for an arc.
        // Character can be main in one arc and secondary in another.
        public bool IsMainCharacter { get; set; }
    }
}
