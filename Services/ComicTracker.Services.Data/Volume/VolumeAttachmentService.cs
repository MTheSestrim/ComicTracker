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

    public class VolumeAttachmentService : IVolumeAttachmentService
    {
        private readonly ComicTrackerDbContext context;

        public VolumeAttachmentService(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> AttachVolumes(AttachSRERequestModel model)
        {
            var volumes = this.context.Volumes
                .Where(i => i.Number >= model.MinRange
                    && i.Number <= model.MaxRange
                    && i.SeriesId == model.SeriesId)
                .ToList();

            if (volumes.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (model.ParentTypeName == nameof(Arc))
            {
                var arc = this.context.Arcs
                    .Include(a => a.ArcsVolumes)
                    .FirstOrDefault(a => a.Id == model.ParentId);

                if (arc == null)
                {
                    throw new ArgumentNullException();
                }

                foreach (var volume in volumes)
                {
                    var av = new ArcVolume { Arc = arc, Volume = volume };

                    arc.ArcsVolumes.Add(av);
                }

                await this.context.SaveChangesAsync();

                return volumes.Count;
            }

            return null;
        }
    }
}
