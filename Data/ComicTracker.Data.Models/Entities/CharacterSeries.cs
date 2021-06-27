namespace ComicTracker.Data.Models.Entities
{
    public class CharacterSeries
    {
        public int CharacterId { get; set; }

        public Character Character { get; set; }

        public int SeriesId { get; set; }

        public Series Series { get; set; }

        // Determines if character should be displayed with top priority for a series.
        // Character can be main in one series and secondary in another.
        public bool IsMainCharacter { get; set; }
    }
}
