namespace ComicTracker.Web.ViewModels.ViewComponents
{
    using System.Collections.Generic;

    using ComicTracker.Services.Data.Models.Entities;

    public class VCEntityLinkingViewModel
    {
        public string Name { get; set; }

        public IReadOnlyCollection<EntityLinkingModel> EntityLinkings { get; set; }
    }
}
