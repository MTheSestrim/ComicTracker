namespace ComicTracker.Services.Data.Genre.Contracts
{
    using System.Collections.Generic;

    public interface IGenreRetrievalService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
