namespace ComicTracker.Services.Data.Models.Entities
{
    using System.Collections.Generic;

    public class EditInfoSeriesRelatedEntityServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public IEnumerable<int> Genres { get; set; }
    }
}
