namespace ComicTracker.Data.Seeding.EntitySeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Models.Entities;

    using Newtonsoft.Json;

    internal class PublisherSeeder : BaseEntitySeeder
    {
        public override async Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Publishers.Any())
            {
                return;
            }

            var jsonContents = File.ReadAllText(this.GetJSONPath());

            var data = JsonConvert.DeserializeObject<IEnumerable<Publisher>>(jsonContents);

            await dbContext.AddRangeAsync(data);

            await dbContext.SaveChangesAsync();
        }
    }
}
