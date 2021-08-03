namespace ComicTracker.Web.Infrastructure
{
    using AutoMapper;
    using ComicTracker.Services.Data.Series.Models;
    using ComicTracker.Web.ViewModels.Series;

    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            this.CreateMap<EditInfoSeriesServiceModel, EditSeriesInputModel>();
        }
    }
}
