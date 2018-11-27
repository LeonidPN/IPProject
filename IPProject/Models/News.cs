using System;
using System.Runtime.Serialization;

namespace IPProject.Models
{
    [DataContract]
    public class News
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        [DataMember]
        public int NumberOfViews { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string UserLogin { get; set; }
    }
}