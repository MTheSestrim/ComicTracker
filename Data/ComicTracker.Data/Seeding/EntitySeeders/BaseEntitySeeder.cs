namespace ComicTracker.Data.Seeding.EntitySeeders
{
    using System;
    using System.Threading.Tasks;

    using ComicTracker.Data.Seeding.EntitySeeders.Contracts;

    using static ComicTracker.Common.GlobalConstants;

    public abstract class BaseEntitySeeder : IEntitySeeder
    {
        public string GetJSONPath()
        {
            string seederName = this.GetType().Name;

            return $"{JSONDataFolderPath}/{seederName.Substring(0, seederName.Length - 6)}.json";
        }

        public abstract Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider);
    }
}
