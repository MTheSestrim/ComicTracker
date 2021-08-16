namespace ComicTracker.Web.Infrastructure
{
    using System.Linq;

    using AutoMapper;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Home;
    using ComicTracker.Services.Data.Series.Models;

    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // EntityLinkingModel mappings
            this.CreateMap<Issue, EntityLinkingModel>();
            this.CreateMap<Volume, EntityLinkingModel>();
            this.CreateMap<Arc, EntityLinkingModel>();

            this.CreateMap<Genre, NameOnlyLinkingModel>();

            this.CreateMap<Series, HomeSeriesServiceModel>();

            this.CreateMap<Series, EditInfoSeriesServiceModel>()
                .ForMember(sm => sm.Genres, x => x.MapFrom(s => s.Genres.Select(g => g.Id)));
            this.CreateMap<Issue, EditInfoSeriesRelatedEntityServiceModel>()
                .ForMember(sm => sm.Genres, x => x.MapFrom(s => s.Genres.Select(g => g.Id)));
            this.CreateMap<Volume, EditInfoSeriesRelatedEntityServiceModel>()
                .ForMember(sm => sm.Genres, x => x.MapFrom(s => s.Genres.Select(g => g.Id)));
            this.CreateMap<Arc, EditInfoSeriesRelatedEntityServiceModel>()
                .ForMember(sm => sm.Genres, x => x.MapFrom(s => s.Genres.Select(g => g.Id)));
        }
    }
}
