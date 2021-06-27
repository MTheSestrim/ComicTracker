namespace ComicTracker.Data.Models.Entities
{
    public class CharacterVolume
    {
        public int CharacterId { get; set; }

        public Character Character { get; set; }

        public int VolumeId { get; set; }

        public Volume Volume { get; set; }

        // Determines if character should be displayed with top priority for a volume.
        // Character can be main in one volume and secondary in another.
        public bool IsMainCharacter { get; set; }
    }
}
