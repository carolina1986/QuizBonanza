using QuizApp.Models;

namespace QuizApp.Services
{
    public interface IScoreService 
    {   
       // This method will save the score AND check if its a high score 
       Score SaveScore(int userId, int quizId, int points);
       // These methods will work as a filter for leaderboards
       IEnumerable<Score> GetGlobalLeaderboard();
       IEnumerable<Score> GetLeaderboardForOneQuiz(int quizId);
       IEnumerable<Score> GetLeaderboardForOneUser(int userId);
       Score? GetHighestScoreForUserInQuiz(int userId, int quizId);
    }
}