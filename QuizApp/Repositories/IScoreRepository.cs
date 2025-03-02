using QuizApp.Models;

namespace QuizApp.Repositories
{
    public interface IScoreRepository : IRepository<Score>
    {
        Score SaveHighScoreToLeaderboard(int userId, int quizId, int points);
        Score SaveScore(int userId, int quizId, int points);
        IEnumerable<Score> GetLeaderboardForAllQuizzes();
        IEnumerable<Score> GetLeaderboardForQuiz(int quizId);
        IEnumerable<Score> GetLeaderboardForUser(int userId);
        IEnumerable<Score> GetAllScoresForUser(int userId);
        Score GetScoreForUserInQuiz(int userId, int quizId);
        Score GetHighestScoreForUserInQuiz(int userId, int quizId);

    }
}