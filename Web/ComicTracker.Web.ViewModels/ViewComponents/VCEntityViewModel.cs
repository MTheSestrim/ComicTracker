namespace ComicTracker.Web.ViewModels.ViewComponents
{
    using System.Collections.Generic;

    public class VCEntityViewModel
    {
        public int Id { get; set; }

        public string EntityTypeName { get; set; }

        public string Description { get; set; }

        public IReadOnlyCollection<VCEntityLinkingViewModel> EntityLinkings { get; set; }
    }
}
