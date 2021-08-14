namespace ComicTracker.Services.Data.Arc.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IArcTemplateCreationService
    {
        int? CreateTemplateArcs(TemplateCreateApiRequestModel model);
    }
}
