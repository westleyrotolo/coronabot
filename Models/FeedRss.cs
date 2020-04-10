using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    [Table("feedrss")]
    public class FeedRss
    {
        [Key]
        public string Link { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public List<FeedItem> FeedItems { get; set; }

    }
}
