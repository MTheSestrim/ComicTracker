namespace ComicTracker.Tests.Data.Genre
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Models.Entities;

    public class GenreSample
    {
        public static IEnumerable<Genre> TenGenresWithIds => Enumerable
            .Range(1, 50)
            .Select(i => new Genre() { Id = i, Name = $"Genre{i}" }).ToList();
    }
}
