using QuizApp.Models;

namespace QuizApp.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Question? GetQuestionWithOptions(int questionId);
        IEnumerable<Question> GetQuestionsByQuizId(int quizId);
        IEnumerable<Question> GetQuestionsWithOptionsByQuizId(int quizId);
    }
}