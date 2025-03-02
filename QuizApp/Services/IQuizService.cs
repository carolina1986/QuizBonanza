using QuizApp.Models;

namespace QuizApp.Services
{
    public interface IQuizService
    {
        Quiz CreateQuiz(string title, string description, int CreatorId, List<Question> questions);
        Quiz GetQuizById(int id);
        IEnumerable<Quiz> GetAllQuizzes();
        IEnumerable<Quiz> GetQuizzesByCreator(int creatorId);    
        Quiz UpdateQuiz(Quiz quiz);
        void RemoveQuiz(int id);
    }
}