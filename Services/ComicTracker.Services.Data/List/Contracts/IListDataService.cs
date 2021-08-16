namespace ComicTracker.Services.Data.List.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Services.Data.List.Models;

    public interface IListDataService
    {
        IEnumerable<ListServiceModel> GetListData(string userId);
    }
}
