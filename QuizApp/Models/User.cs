using System;
using System.Collections.Generic;

namespace QuiApp.Models
{

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<Quiz> CreatedQuizzes { get; set; }
        public List<Score> Scores { get; set; }
    }
}