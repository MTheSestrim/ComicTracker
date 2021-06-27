namespace ComicTracker.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider);
    }
}
