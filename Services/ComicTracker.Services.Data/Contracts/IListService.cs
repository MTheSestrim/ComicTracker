namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Services.Data.Models.List;

    public interface IListService
    {
        IEnumerable<ListServiceModel> GetListData(string userId);
    }
}
