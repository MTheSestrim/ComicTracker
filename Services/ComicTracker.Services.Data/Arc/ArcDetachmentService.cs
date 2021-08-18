namespace ComicTracker.Services.Data.Arc
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class ArcDetachmentService : IArcDetachmentService
    {
        private readonly ComicTrackerDbContext context;

        public ArcDetachmentService(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> DetachArcs(AttachSRERequestModel model)
        {
            var arcsVolumes = this.context.ArcVolumes
                .Where(i => i.Arc.Number >= model.MinRange
                    && i.Arc.Number <= model.MaxRange
                    && i.Arc.SeriesId == model.SeriesId
                    && i.VolumeId == model.ParentId)
                .ToList();

            if (arcsVolumes.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (model.ParentTypeName == nameof(Volume))
            {
                var volume = this.context.Volumes
                                .Include(v => v.ArcsVolumes)
                                .FirstOrDefault(v => v.Id == model.ParentId);

                foreach (var av in arcsVolumes)
                {
                    volume.ArcsVolumes.Remove(av);
                }

                await this.context.SaveChangesAsync();

                return arcsVolumes.Count;
            }

            return null;
        }
    }
}
