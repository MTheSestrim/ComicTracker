namespace ComicTracker.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "ComicTracker";

        public const string AdministratorRoleName = "Administrator";

        public const string JSONDataFolderPath = "../../Data/ComicTracker.Data/Seeding/JSONData";

        public const string SeriesImagePath = "/images/series/";

        public const int SeriesPerRow = 4;

        // {0} -> Series Title; {1} -> Issue Number
        public const string DefaultSeriesRelatedEntityTitleFormat = "{0} #{1}";

        public const string DefaultDescription = "No description.";

        public const int DefaultEntityTitleLength = 150;

        public const int DefaultSeriesNameMaxLength = 200;

        public const int DefaultSeriesNameMinLength = 2;

        public const int DefaultImageSizeInKB = 2048;

        public const int BytesInAKilobyte = 1000;
    }
}
