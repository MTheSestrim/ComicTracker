namespace ComicTracker.Tests.Services.Arc
{
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    public class MockArcTemplateCreationService : IArcTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockArcTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateTemplateArcs(TemplateCreateApiRequestModel model)
        {
            if (model.NumberOfEntities < 1)
            {
                return -1;
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
