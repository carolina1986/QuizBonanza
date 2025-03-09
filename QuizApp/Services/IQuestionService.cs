using QuizApp.Models;

namespace QuizApp.Services
{
    public interface IQuestionService
    {
        Question AddQuestionWithOptionsToQuiz(int quizId, string questionText, List<Option> options);
        Question? GetQuestionById(int id);
        IEnumerable<Question> GetQuestionsByQuiz(int quizId);
        void UpdateQuestion(Question question);
        void UpdateQuestionText(int id, string newQuestionText);
        void DeleteQuestion(int id);
        Option AddOptionToQuestion(int questionId, string optionText, bool isCorrect);
        Option? GetOptionById(int optionId);
        void UpdateOption(int optionId, string newOptionText, bool isCorrect);
        void DeleteOption(int optionId);
    }
}