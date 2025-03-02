using QuizApp.Models;
using QuizApp.Repositories;

namespace QuizApp.Services
{
    public class ScoreService : IScoreService
    {
        private readonly IScoreRepository _scoreRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;

        public ScoreService(
            IScoreRepository scoreRepository,
            IQuizRepository quizRepository,
            IUserRepository userRepository)
        {
            _scoreRepository = scoreRepository;
            _quizRepository = quizRepository;   
            _userRepository = userRepository;
        }

        public Score SaveScore (int userId, int quizId, int points)
        {
            User user = _userRepository.GetById(userId);
            Quiz quiz = _quizRepository.GetById(quizId);

            if (user == null || quiz == null)
            {
                throw new Exception("User or quiz ID not found");
            }

            return _scoreRepository.SaveHighScoreToLeaderboard(userId, quizId, points);
        }

        public IEnumerable<Score> GetGlobalLeaderboard()
        {
            return _scoreRepository.GetLeaderboardForAllQuizzes();
        }

        public IEnumerable<Score> GetLeaderboardForOneQuiz(int quizId)
        {
            Quiz quiz = _quizRepository.GetById(quizId);
            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }
            return _scoreRepository.GetLeaderboardForQuiz(quizId);
        }

        public IEnumerable<Score> GetLeaderboardForOneUser(int userId)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _scoreRepository.GetLeaderboardForUser(userId);
        }

        public Score GetHighestScoreForUserInQuiz(int userId, int quizId)
        {
            return _scoreRepository.GetHighestScoreForUserInQuiz(userId, quizId);
        }

    }
}