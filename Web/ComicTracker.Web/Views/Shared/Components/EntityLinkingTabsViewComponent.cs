namespace ComicTracker.Web.Views.Shared.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ComicTracker.Web.ViewModels.Contracts;
    using ComicTracker.Web.ViewModels.Entities;
    using ComicTracker.Web.ViewModels.ViewComponents;

    using Microsoft.AspNetCore.Mvc;

    public class EntityLinkingTabsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEntityViewModel entityModel)
        {
            var items = this.GetEntityLinkingModels(entityModel);

            var vcModel = new VCEntityViewModel
            {
                Description = entityModel.Description,
                EntityLinkings = items,
            };

            return this.View(vcModel);
        }

        private ICollection<VCEntityLinkingViewModel> GetEntityLinkingModels(
            IEntityViewModel entityModel)
        {
            return entityModel.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(ICollection<EntityLinkingModel>))
                .Select(p => new VCEntityLinkingViewModel
                {
                    Name = p.Name,
                    EntityLinkings = (ICollection<EntityLinkingModel>)p.GetValue(entityModel),
                })
                .ToList();
        }
    }
}
