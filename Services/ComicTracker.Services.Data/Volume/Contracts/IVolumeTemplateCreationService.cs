namespace ComicTracker.Services.Data.Volume.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IVolumeTemplateCreationService
    {
        int? CreateTemplateVolumes(TemplateCreateApiRequestModel model);
    }
}
