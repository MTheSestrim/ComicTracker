namespace ComicTracker.Data.Seeding.EntitySeeders.Contracts
{
    public interface IEntitySeeder : ISeeder
    {
        string GetJSONPath();
    }
}
