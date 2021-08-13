namespace ComicTracker.Services
{
    using System.Collections.Generic;

    using ComicTracker.Services.Contracts;

    public class CacheKeyHolderService : ICacheKeyHolderService<int>
    {
        public CacheKeyHolderService()
        {
            this.Keys = new Dictionary<string, IList<int>>();
        }

        private IDictionary<string, IList<int>> Keys { get; init; }

        public IList<int> GetKeys(string typeName)
        {
            if (this.Keys.ContainsKey(typeName))
            {
                return this.Keys[typeName];
            }

            return null;
        }

        public void AddKey(string typeName, int key)
        {
            if (!this.Keys.ContainsKey(typeName))
            {
                this.Keys[typeName] = new List<int>();
            }

            this.Keys[typeName].Add(key);
        }

        public void Clear(string typeName)
        {
            if (!this.Keys.ContainsKey(typeName))
            {
                this.Keys[typeName].Clear();
            }
        }
    }
}
