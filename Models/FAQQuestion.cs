using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    public class FAQQuestion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Question { get; set; }
    }
}
