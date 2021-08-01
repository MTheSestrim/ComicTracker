namespace ComicTracker.Services.Data.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RateApiRequestModel
    {
        public int Id { get; set; }

        [Range(0, 10, ErrorMessage = "Score must be an integer between 0 and 10")]
        public int Score { get; set; }
    }
}
