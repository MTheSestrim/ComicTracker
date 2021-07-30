namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Web.ViewModels.List;

    public interface IListService
    {
        IEnumerable<ListViewModel> GetListData(string userId);
    }
}
