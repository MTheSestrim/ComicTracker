namespace ComicTracker.Services.Contracts
{
    using System;

    public interface IEntityNameExtractService
    {
        string ExtractEntityTypeName(Type entityType);
    }
}
