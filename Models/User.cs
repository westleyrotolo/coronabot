using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    public class User
    {
        public string Id { get; set; }
        public int State { get; set; }
        public List<SelfCertification> SelfCertifications { get; set; }
    }
}
