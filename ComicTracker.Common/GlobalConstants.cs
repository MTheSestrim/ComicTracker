﻿namespace ComicTracker.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "ComicTracker";

        public const string AdministratorRoleName = "Administrator";

        public const string JSONDataFolderPath = "../../Data/ComicTracker.Data/Seeding/JSONData";

        public const string SeriesImagePath = "/images/series/";

        public const int SeriesPerRow = 4;

        // {0} -> Series Title; {1} -> Issue Number
        public const string DefaultIssueTitleFormat = "{0} #{1}";
    }
}
