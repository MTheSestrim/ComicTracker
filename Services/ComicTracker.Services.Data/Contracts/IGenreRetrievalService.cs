namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;

    public interface IGenreRetrievalService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
