namespace ComicTracker.Data.Seeding
{
    using static ComicTracker.Common.GlobalConstants;

    public class JSONPathSupplier
    {
        public string GetJSONPath(ISeeder seeder)
        {
            string seederName = seeder.GetType().Name;

            return $"{JSONDataFolderPath}/{seederName.Substring(0, seederName.Length - 6)}.json";
        }
    }
}
