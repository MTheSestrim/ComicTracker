namespace ComicTracker.Data.Seeding.EntitySeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Models.Entities;

    using Newtonsoft.Json;

    internal class NationalitySeeder : BaseEntitySeeder
    {
        public override async Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Nationalities.Any())
            {
                return;
            }

            var jsonContents = File.ReadAllText(this.GetJSONPath());

            var data = JsonConvert.DeserializeObject<IEnumerable<Nationality>>(jsonContents);

            await dbContext.AddRangeAsync(data);

            await dbContext.SaveChangesAsync();
        }
    }
}
