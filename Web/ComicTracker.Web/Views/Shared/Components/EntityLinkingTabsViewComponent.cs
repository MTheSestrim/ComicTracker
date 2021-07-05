namespace ComicTracker.Web.Views.Shared.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ComicTracker.Web.ViewModels.Contracts;
    using ComicTracker.Web.ViewModels.Entities;
    using ComicTracker.Web.ViewModels.ViewComponents;

    using Microsoft.AspNetCore.Mvc;

    using static ComicTracker.Common.GlobalConstants;

    public class EntityLinkingTabsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEntityViewModel entityModel)
        {
            var items = this.GetEntityLinkingModels(entityModel);

            if (entityModel.Description == null)
            {
                entityModel.Description = DefaultDescription;
            }

            var vcModel = new VCEntityViewModel
            {
                Description = entityModel.Description,
                EntityLinkings = items,
            };

            return this.View(vcModel);
        }

        private IReadOnlyCollection<VCEntityLinkingViewModel> GetEntityLinkingModels(
            IEntityViewModel entityModel)
        {
            return entityModel.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(IReadOnlyCollection<EntityLinkingModel>))
                .Select(p => new VCEntityLinkingViewModel
                {
                    Name = p.Name,
                    EntityLinkings = (IReadOnlyCollection<EntityLinkingModel>)p.GetValue(entityModel),
                })
                .ToList();
        }
    }
}
