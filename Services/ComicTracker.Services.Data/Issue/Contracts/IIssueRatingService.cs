﻿namespace ComicTracker.Services.Data.Issue.Contracts
{
    using System.Threading.Tasks;

    public interface IIssueRatingService
    {
        Task<int> RateIssue(string userId, int issueId, int score);
    }
}