namespace ComicTracker.Web.Views.Shared.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Models.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.ViewModels.ViewComponents;

    using Microsoft.AspNetCore.Mvc;

    using static ComicTracker.Common.GlobalConstants;

    public class EntityLinkingTabsViewComponent : ViewComponent
    {
        private readonly IEntityNameExtractService entityNameExtractService;

        public EntityLinkingTabsViewComponent(IEntityNameExtractService entityNameExtractService)
        {
            this.entityNameExtractService = entityNameExtractService;
        }

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
                EntityTypeName = this.entityNameExtractService.ExtractEntityTypeName(entityModel.GetType()),
                Description = entityModel.Description,
                EntityLinkings = items,
            };

            return this.View(vcModel);
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
