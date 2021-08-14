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

    public class ArcAttachmentService : IArcAttachmentService
    {
        private readonly ComicTrackerDbContext context;

        public ArcAttachmentService(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> AttachArcs(AttachSRERequestModel model)
        {
            var arcs = this.context.Arcs
                .Where(i => i.Number >= model.MinRange
                    && i.Number <= model.MaxRange
                    && i.SeriesId == model.SeriesId)
                .ToList();

            if (arcs == null)
            {
                throw new ArgumentOutOfRangeException("Incorrect issue range given.");
            }

            if (model.ParentTypeName == nameof(Volume))
            {
                var volume = this.context.Volumes
                    .Include(v => v.ArcsVolumes)
                    .FirstOrDefault(v => v.Id == model.ParentId);

                if (volume == null)
                {
                    throw new ArgumentNullException($"Volume with given id {model.ParentId} does not exist.");
                }

                foreach (var arc in arcs)
                {
                    var av = new ArcVolume { Arc = arc, Volume = volume };

                    volume.ArcsVolumes.Add(av);
                }

                await this.context.SaveChangesAsync();

                return arcs.Count;
            }

            return null;
        }
    }
}
