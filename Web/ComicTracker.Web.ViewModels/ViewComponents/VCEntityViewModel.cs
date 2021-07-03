namespace ComicTracker.Web.ViewModels.ViewComponents
{
    using System.Collections.Generic;

    public class VCEntityViewModel
    {
        public string Description { get; set; }

        public IEnumerable<VCEntityLinkingViewModel> EntityLinkings { get; set; }
    }
}
