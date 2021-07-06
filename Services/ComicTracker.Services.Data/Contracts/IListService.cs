namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ComicTracker.Web.ViewModels.List;

    public interface IListService
    {
        Task<IEnumerable<ListViewModel>> GetListDataAsync();
    }
}
