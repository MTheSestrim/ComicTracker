namespace ComicTracker.Data.Seeding.EntitySeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Models.Entities;

    using Newtonsoft.Json;

    internal class IssueSeeder : BaseEntitySeeder
    {
        public override async Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Issues.Any())
            {
                return;
            }

            var jsonContents = File.ReadAllText(this.GetJSONPath());

            var data = JsonConvert.DeserializeObject<IEnumerable<Issue>>(jsonContents);

            await dbContext.AddRangeAsync(data);

            await dbContext.SaveChangesAsync();
        }
    }
}
