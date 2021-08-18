namespace ComicTracker.Tests.Mocks.Services.Arc
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    public class MockArcAttachmentService : IArcAttachmentService
    {
        private readonly ComicTrackerDbContext context;

        public MockArcAttachmentService(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> AttachArcs(AttachSRERequestModel model)
        {
            var arcs = this.context.Arcs
                .ToList()
                .Where(i => i.Number >= model.MinRange
                    && i.Number <= model.MaxRange
                    && i.SeriesId == model.SeriesId)
                .ToList();

            if (arcs.Count() == 0)
            {
                throw new ArgumentOutOfRangeException("Incorrect arc range given.");
            }

            if (model.ParentTypeName == nameof(Volume))
            {
                var volume = this.context.Volumes
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

                return arcs.Count();
            }

            return null;
        }
    }
}
