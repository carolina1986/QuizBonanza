using QuizApp.Models;
using QuizApp.Data;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Repositories 
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(QuizDbContext context) : base(context)
        {
        }

        public Question? GetQuestionWithOptions(int questionId)
        {
            return _context.Questions // This will return all the questions in the database
                .Include(q => q.Options) // This will include the options for each question
                .FirstOrDefault(q => q.Id == questionId); // This will return the question with the given id
        }

        public IEnumerable<Question> GetQuestionsByQuizId(int quizId)
        {
            return _context.Questions
                .Where(q => q.QuizId == quizId) // This will return all the questions with the given quiz id
                .Include(q => q.Options)
                .ToList(); 
        }

        public IEnumerable<Question> GetQuestionsWithOptionsByQuizId(int quizId)
        {
            return _context.Questions
                .Include(q => q.Options)
                .Where(q => q.QuizId == quizId)
                .ToList();
        }
    }
}