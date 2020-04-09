using System;
using System.Collections.Generic;
using CoronaBot.Models;

namespace CoronaBot.ViewModels
{
    public class UserQuestionSearchResponseViewModel
    {
        public int Count { get; set; }
        public List<UserQuestion> Questions{ get; set; }
    }
}
