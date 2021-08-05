namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    public class VolumeController : BaseController
    {
        private readonly IVolumeDetailsService volumeDetailsService;

        public VolumeController(IVolumeDetailsService volumeDetailsService)
        {
            this.volumeDetailsService = volumeDetailsService;
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
    }
}
