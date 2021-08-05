namespace ComicTracker.Data
{
    using System;

    using ComicTracker.Data.Common.Models;

    public static class ComicTrackerDbContextExtensions
    {
        public static void HardDelete(this ComicTrackerDbContext dbContext, BaseModel<int> entity)
            => dbContext.Remove(entity);

        public static void Undelete(this ComicTrackerDbContext dbContext, BaseDeletableModel<int> entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            dbContext.Update(entity);
        }

        public static void Delete(this ComicTrackerDbContext dbContext, BaseDeletableModel<int> entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            dbContext.Update(entity);
        }
    }
}
