namespace ComicTracker.Services.Contracts
{
    using System.Collections.Generic;

    public interface ICacheKeyHolderService<T>
    {
        public IList<int> GetKeys(string typeName);

        public void AddKey(string typeName, int key);

        public void Clear(string typeName);
    }
}
