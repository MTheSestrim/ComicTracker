namespace ComicTracker.Data.Seeding.EntitySeeders
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using ComicTracker.Data.Seeding.EntitySeeders.Contracts;

    using static ComicTracker.Common.GlobalConstants;

    public abstract class BaseEntitySeeder : IEntitySeeder
    {
        public string GetJSONPath()
        {
            string seederName = this.GetType().Name;

            var developmentPath = $"{JSONDataFolderPath}/{seederName[0..^6]}.json";

            /* Since the build directory is different for the test project,
             additional backtracking is necessary to reach the .json files. */
            var developmentConditional = File.Exists($"{JSONDataFolderPath}/{seederName[0..^6]}.json");

            if (!developmentConditional)
            {
                return $"../../../{JSONDataFolderPath}/{seederName[0..^6]}.json";
            }

            return developmentPath;
        }

        public abstract Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider);
    }
}
