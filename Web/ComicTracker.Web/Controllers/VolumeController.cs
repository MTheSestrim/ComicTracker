namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Volume.Contracts;

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
            var currentVolume = this.volumeDetailsService.GetVolume(id);

            if (currentVolume == null)
            {
                return this.NotFound(currentVolume);
            }

            return this.View(currentVolume);
        }
    }
}
