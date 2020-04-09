using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    public class UserQuestion
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Intent { get; set; }
        public double Score { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
