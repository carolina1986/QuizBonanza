using System;
using System.Collections.Generic;
using QuiApp.Models;

namespace QuizApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public List<Question> Questions { get; set; }
        public List<Score> Scores { get; set; }
    }
}