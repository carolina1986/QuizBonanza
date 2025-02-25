
namespace QuizApp.Models
{

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<Quiz> CreatedQuizzes { get; set; } = new List<Quiz>();
        public List<Score> Scores { get; set; } = new List<Score>();
    }
}