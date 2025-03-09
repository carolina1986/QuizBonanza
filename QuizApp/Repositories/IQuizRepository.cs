using QuizApp.Models;

namespace QuizApp.Repositories
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Quiz? GetQuizWithQuestions(int quizId);
        Quiz? GetQuizWithQuestionsAndOptions(int quizId);
        IEnumerable<Quiz> GetQuizzesByCreator(int creatorId);
    }
}