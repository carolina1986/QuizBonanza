using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using QuizApp.Data;

namespace QuizApp.Repositories
{
    public class ScoreRepository : Repository<Score>, IScoreRepository
    {
        public ScoreRepository(QuizDbContext context) : base(context)
        {
        }

        public Score SaveHighScoreToLeaderboard(int userId, int quizId, int points)
        {
            var existingHighScore = _context.Scores
                .Where(s => s.UserId == userId && s.QuizId == quizId)
                .OrderByDescending(s => s.Points)
                .FirstOrDefault();

            if (existingHighScore == null || existingHighScore.Points < points)
            {
                var newHighScore = new Score
                {
                    UserId = userId,
                    QuizId = quizId, // Se till att rätt quiz-ID sätts
                    Points = points,
                    PlayedAt = DateTime.UtcNow
                };

                _context.Scores.Add(newHighScore);
                _context.SaveChanges();

                return newHighScore;
            }

            return existingHighScore;
        }

        public Score SaveScore(int userId, int quizId, int points)
        {
            var newScore = new Score
            {
                UserId = userId,
                QuizId = quizId,
                Points = points,
                PlayedAt = DateTime.Now
            };

            _context.Scores.Add(newScore);
            _context.SaveChanges();

            return newScore;
        }


        public IEnumerable<Score> GetLeaderboardForAllQuizzes()
        {
            return _context.Scores
                .Include(s => s.User)
                .Include(s => s.Quiz)
                .OrderByDescending(s => s.Points)
                .ToList();
        }

        public IEnumerable<Score> GetLeaderboardForQuiz(int quizId)
        {
            var scores = _context.Scores
                .Where(s => s.QuizId == quizId)
                .Include(s => s.User) 
                //.Include(s => s.Quiz)
                .OrderByDescending(s => s.Points)
                .ToList();

            return scores;
        }

        public IEnumerable<Score> GetLeaderboardForUser(int userId)
        {
            return _context.Scores
                .Include(s => s.User)
                .Include(s => s.Quiz)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.Points)
                .ToList();
        }

        public IEnumerable<Score> GetAllScoresForUser(int userId)
        {
            return _context.Scores
                .Include(s => s.User)
                .Include(s => s.Quiz)
                .Where(s => s.UserId == userId)
                .ToList();
        }

        public Score? GetScoreForUserInQuiz(int userId, int quizId)
        {
            return _context.Scores
                .Include(s => s.User)
                .Include(s => s.Quiz)
                .FirstOrDefault(s => s.UserId == userId && s.QuizId == quizId);
        }

        public Score? GetHighestScoreForUserInQuiz(int userId, int quizId)
        {
            return _context.Scores
                .Include(s => s.User)
                .Include(s => s.Quiz)
                .Where(s => s.UserId == userId && s.QuizId == quizId)
                .OrderByDescending(s => s.Points)
                .FirstOrDefault();
        }
    }
}