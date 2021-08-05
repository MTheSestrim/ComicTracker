namespace ComicTracker.Web.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class VolumeController : BaseController
    {
        private readonly IVolumeDetailsService volumeDetailsService;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IVolumeCreationService volumeCreationService;

        public VolumeController(
            IVolumeDetailsService volumeDetailsService,
            IGenreRetrievalService genreRetrievalService,
            IVolumeCreationService volumeCreationService)
        {
            this.volumeDetailsService = volumeDetailsService;
            this.genreRetrievalService = genreRetrievalService;
            this.volumeCreationService = volumeCreationService;
        }

        public IActionResult Index(int id)
        {
            var currentVolume = this.volumeDetailsService.GetVolume(id, this.User.GetId());

            if (currentVolume == null)
            {
                return this.NotFound(currentVolume);
            }

            return this.View(currentVolume);
        }

        [Authorize]
        public IActionResult Create(int id, int number = 0)
        {
            var viewModel = new CreateSeriesRelatedEntityInputModel();
            viewModel.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
            viewModel.SeriesId = id;
            viewModel.Number = number;

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateSeriesRelatedEntityInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var serviceModel = new CreateSeriesRelatedEntityServiceModel
            {
                Title = model.Title,
                Number = model.Number,
                CoverImage = await model.CoverImage.GetBytes(),
                CoverPath = model.CoverPath,
                Description = model.Description,
                Genres = model.Genres,
                SeriesId = model.SeriesId,
                RetrievedGenres = model.RetrievedGenres,
            };

            var id = this.volumeCreationService.CreateVolume(serviceModel);

            return this.Redirect($"/Volume/{id}");
        }
    }
}
