namespace ComicTracker.Web.Infrastructure
{
    using AutoMapper;

    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Models;
    using ComicTracker.Web.ViewModels.Entities;
    using ComicTracker.Web.ViewModels.Series;

    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            this.CreateMap<EditInfoSeriesServiceModel, EditSeriesInputModel>();
            this.CreateMap<EditInfoSeriesRelatedEntityServiceModel, EditSeriesRelatedEntityInputModel>();
        }
    }
}
