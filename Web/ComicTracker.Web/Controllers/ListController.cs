namespace ComicTracker.Web.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ListController : BaseController
    {
        private readonly IListService listService;

        public ListController(IListService listService)
        {
            this.listService = listService;
        }

        public async Task<IActionResult> Index()
        {
            var listData = await this.listService.GetListDataAsync();

            return this.View(listData);
        }
    }
}
