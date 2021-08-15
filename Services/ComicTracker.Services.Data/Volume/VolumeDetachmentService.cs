namespace ComicTracker.Services.Data.Volume
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class VolumeDetachmentService : IVolumeDetachmentService
    {
        private readonly ComicTrackerDbContext context;

        public VolumeDetachmentService(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> DetachVolumes(AttachSRERequestModel model)
        {
            var arcsVolumes = this.context.ArcVolumes
                .Where(i => i.Volume.Number >= model.MinRange
                    && i.Volume.Number <= model.MaxRange
                    && i.Volume.SeriesId == model.SeriesId
                    && i.ArcId == model.ParentId)
                .ToList();

            if (arcsVolumes == null)
            {
                throw new ArgumentOutOfRangeException("Incorrect volume range given.");
            }

            if (model.ParentTypeName == nameof(Arc))
            {
                var arc = this.context.Arcs
                                .Include(a => a.ArcsVolumes)
                                .FirstOrDefault(a => a.Id == model.ParentId);

                if (arc == null)
                {
                    throw new ArgumentNullException($"Arc with given id {model.ParentId} does not exist.");
                }

                foreach (var av in arcsVolumes)
                {
                    arc.ArcsVolumes.Remove(av);
                }

                await this.context.SaveChangesAsync();

                return arcsVolumes.Count;
            }

            return null;
        }
    }
}
