using QuizApp.Models;
using QuizApp.Repositories;

namespace QuizApp.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;

        public QuizService(IQuizRepository quizRepository, IUserRepository userRepository)
        {
            _quizRepository = quizRepository;
            _userRepository = userRepository;
        }

        public Quiz CreateQuiz(string title, string description, int CreatorId, List<Question> questions)
        {
            var creator = _userRepository.GetById(CreatorId);
            if (creator == null)
            {
                throw new Exception("User not found");
            }

            var quiz = new Quiz
            {
                Title = title,
                Description = description,
                CreatorId = CreatorId,
            };

            _quizRepository.Add(quiz); //LAGT TILL
            _quizRepository.SaveChanges(); //LAGT TILL

            if (questions != null)
            {
                foreach (var question in questions)
                {
                    question.QuizId = quiz.Id; // Sätt rätt quiz-ID för varje fråga
                }

                quiz.Questions = questions;
            }

            _quizRepository.SaveChanges();

            return quiz;
        }


        public Quiz? GetQuizById(int id)
        {
            return _quizRepository.GetQuizWithQuestionsAndOptions(id);
        }

        public IEnumerable<Quiz> GetAllQuizzes()
        {
            return _quizRepository.GetAll(); 
        }

        public IEnumerable<Quiz> GetQuizzesByCreator(int creatorId)
        {
            return _quizRepository.GetQuizzesByCreator(creatorId).ToList();
        }

        public Quiz UpdateQuiz(Quiz quiz)
        {
            _quizRepository.Update(quiz);
            _quizRepository.SaveChanges();
            return quiz;
        }

        public void RemoveQuiz(int id)
        {
            var quiz = _quizRepository.GetQuizWithQuestionsAndOptions(id);
            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }

            _quizRepository.Remove(quiz);
            _quizRepository.SaveChanges();
        }
    }
}