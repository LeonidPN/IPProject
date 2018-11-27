using System;

namespace IPProject.Models
{
    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateCreate { get; set; }

        public int NumberOfViews { get; set; }

        public int CategoryId { get; set; }

        public string UserLogin { get; set; }
    }
}