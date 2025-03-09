using QuizApp.Models;
using QuizApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Services
{

    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IQuizRepository _quizRepository;

        public QuestionService(
            IQuestionRepository questionRepository,
            IOptionRepository optionRepository,
            IQuizRepository quizRepository)
        {
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _quizRepository = quizRepository;
        }

        public Question AddQuestionWithOptionsToQuiz(int quizId, string questionText, List<Option> options)
        {
            Quiz? quiz = _quizRepository.GetById(quizId);
            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }

            Question question = new Question
            {
                Text = questionText,
                QuizId = quizId,
                Options = new List<Option>()
            };

            _questionRepository.Add(question);
            _questionRepository.SaveChanges();

            if (options != null && options.Any())
            {
                foreach (Option option in options)
                {
                    option.QuestionId = question.Id;
                    _optionRepository.Add(option);
                }
                _optionRepository.SaveChanges();
            }
            return question;
        }

        public Question? GetQuestionById(int id)
        {
            return _questionRepository.GetById(id);
        }

        public IEnumerable<Question> GetQuestionsByQuiz(int quizId)
        {
            return _questionRepository
                .Find(q => q.QuizId == quizId)
                .Include(q => q.Options) // Ensure options are loaded
                .ToList();
        }

        public void UpdateQuestion(Question question)
        {
            _questionRepository.Update(question);
            _questionRepository.SaveChanges();
        }

        public void UpdateQuestionText(int questionId, string newQuestionText)
        {
            Question? question = _questionRepository.GetById(questionId);
            if (question == null)
            {
                throw new Exception("Question not found");
            }

            question.Text = newQuestionText;
            _questionRepository.Update(question);
            _questionRepository.SaveChanges();
        }

        public void DeleteQuestion(int questionId)
        {
            Question? question = _questionRepository.GetById(questionId);
            if (question == null)
            {
                throw new Exception("Question not found");
            }

            IEnumerable<Option> options = _optionRepository.Find(o => o.QuestionId == questionId);
            if (options.Any())
            {
                _optionRepository.RemoveRange(options);
                _optionRepository.SaveChanges();
            }

            _questionRepository.Remove(question);
            _questionRepository.SaveChanges();
        }

        public Option AddOptionToQuestion(int questionId, string optionText, bool isCorrect)
        {
            Question? question = _questionRepository.GetById(questionId);
            if (question == null)
            {
                throw new Exception("Question not found");
            }

            Option option = new Option
            {
                Text = optionText,
                IsCorrect = isCorrect,
                QuestionId = questionId
            };

            _optionRepository.Add(option);
            _optionRepository.SaveChanges();

            return option;
        }

        public Option? GetOptionById(int optionId)
        {
            return _optionRepository.GetById(optionId);
        }

        public void UpdateOption(int optionId, string optionText, bool isCorrect)
        {
            Option? option = _optionRepository.GetById(optionId);
            if (option == null)
            {
                throw new Exception("Option not found");
            }

            option.Text = optionText;
            option.IsCorrect = isCorrect;

            _optionRepository.Update(option);
            _optionRepository.SaveChanges();
        }

        public void DeleteOption(int optionId)
        {
            Option? option = _optionRepository.GetById(optionId);
            if (option == null)
            {
                throw new Exception("Option not found");
            }

            _optionRepository.Remove(option);
            _optionRepository.SaveChanges();
        }
    }
}