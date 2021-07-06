namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenreRetrievalService
    {
        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();
    }
}
