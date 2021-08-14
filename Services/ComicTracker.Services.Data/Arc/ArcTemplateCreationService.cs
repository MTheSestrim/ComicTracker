namespace ComicTracker.Services.Data.Arc
{
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class ArcTemplateCreationService : IArcTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ArcTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int? CreateTemplateArcs(TemplateCreateApiRequestModel model)
        {
            var arcsWithSeriesId = this.dbContext.Series.Include(s => s.Arcs).Select(s => new { s.Id, s.Arcs }).FirstOrDefault(s => s.Id == model.SeriesId);

            if (arcsWithSeriesId.Arcs.Any() || model.NumberOfEntities < 1)
            {
                return null;
            }

            var templateArcs = new Arc[model.NumberOfEntities];

            for (int i = 0; i < model.NumberOfEntities; i++)
            {
                var templateArc = new Arc
                {
                    Number = i + 1,
                    SeriesId = model.SeriesId,
                };

                templateArcs[i] = templateArc;
            }

            this.dbContext.Arcs.AddRange(templateArcs);
            this.dbContext.SaveChanges();

            return model.NumberOfEntities;
        }
    }
}
