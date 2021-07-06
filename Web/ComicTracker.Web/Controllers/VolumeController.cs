namespace ComicTracker.Web.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class VolumeController : BaseController
    {
        private readonly IVolumeDetailsService volumeDetailsService;

        public VolumeController(IVolumeDetailsService volumeDetailsService)
        {
            this.volumeDetailsService = volumeDetailsService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var currentVolume = await this.volumeDetailsService.GetVolumeAsync(id);

            if (currentVolume == null)
            {
                return this.NotFound(currentVolume);
            }

            return this.View(currentVolume);
        }
    }
}
