using System;

namespace QuizApp.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public DateTime PlayedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
}