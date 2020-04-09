using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    public class FAQAnswer
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Answer { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
    }
}
