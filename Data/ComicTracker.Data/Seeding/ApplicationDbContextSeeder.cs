namespace ComicTracker.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ComicTracker.Data.Seeding.EntitySeeders;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new UsersSeeder(),
                              new SettingsSeeder(),
                              new ArcSeeder(),
                              new ArtistSeeder(),
                              new CharacterSeeder(),
                              new GenreSeeder(),
                              new IssueSeeder(),
                              new NationalitySeeder(),
                              new PublisherSeeder(),
                              new VolumeSeeder(),
                              new WriterSeeder(),
                              new SeriesSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
