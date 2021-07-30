namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ListController : BaseController
    {
        private readonly IListService listService;

        public ListController(IListService listService)
        {
            this.listService = listService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var listData = this.listService.GetListData(this.User.GetId());

            return this.View(listData);
        }
    }
}
