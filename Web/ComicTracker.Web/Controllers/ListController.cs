namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ListController : BaseController
    {
        private readonly IListService listService;

        public ListController(IListService listService)
        {
            this.listService = listService;
        }

        public IActionResult Index()
        {
            var listData = this.listService.GetListData();

            return this.View(listData);
        }
    }
}
