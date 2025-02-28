using QuizApp.Models;

namespace QuizApp.Repositories
{
    public interface IOptionRepository : IRepository<Option>
    {
        IEnumerable<Option> GetOptionsByQuestionId(int QuestionId);
        Option GetCorrectOptionByQuestionId(int QuestionId);
        Option GetOptionByOptionId(int OptionId);
    }
}