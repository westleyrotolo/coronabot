using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidBot.Models
{

    [Table("visitor")]
    public class Visitor
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Who { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
