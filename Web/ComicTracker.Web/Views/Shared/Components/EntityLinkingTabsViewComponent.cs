namespace ComicTracker.Web.Views.Shared.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ComicTracker.Services.Data.Models.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.ViewModels.ViewComponents;

    using Microsoft.AspNetCore.Mvc;

    using static ComicTracker.Common.GlobalConstants;

    public class EntityLinkingTabsViewComponent : ViewComponent
    {
        // Not extracted in a separate constant class since it's only used here and it's a very specific constant
        private const string ServiceModelSuffix = "DetailsServiceModel";

        public IViewComponentResult Invoke(IEntityServiceModel entityModel)
        {
            var items = this.GetEntityLinkingModels(entityModel);

            if (entityModel.Description == null)
            {
                entityModel.Description = DefaultDescription;
            }

            var vcModel = new VCEntityViewModel
            {
                Id = entityModel.Id,
                EntityTypeName = this.GetEntityTypeName(entityModel.GetType()),
                Description = entityModel.Description,
                EntityLinkings = items,
            };

            return this.View(vcModel);
        }

        private string GetEntityTypeName(Type entityType)
        {
            string entityTypeName = entityType.Name;

            if (!string.IsNullOrWhiteSpace(entityTypeName))
            {
                int charLocation = entityTypeName.IndexOf(ServiceModelSuffix, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return entityTypeName.Substring(0, charLocation);
                }
            }

            return string.Empty;
        }

        private IReadOnlyCollection<VCEntityLinkingViewModel> GetEntityLinkingModels(
            IEntityServiceModel entityModel)
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
