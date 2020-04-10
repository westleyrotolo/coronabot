using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    [Table("feeditems")]
    public class FeedItem
    {
        [Key]
        public string Link { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Media { get; set; }
        public DateTime PubDate { get; set; }
        public string Author { get; set; }
    }
}
