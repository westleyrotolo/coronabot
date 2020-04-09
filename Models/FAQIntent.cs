using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    public class FAQIntent
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Intent { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public List<FAQQuestion> FAQQuestions { get; set; }
        public List<FAQAnswer> FAQAnswers { get; set; }
        public ResponseType ResponseType { get; set; }
    }
    public enum ResponseType
    {
        SEQUENCE,
        RANDOM
    }
}
