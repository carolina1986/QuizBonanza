using System;
using System.Collections.Generic;
using System.Linq;
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
            var existingHighScore = _context.Scores // This will get all the scores from the database
                .Where(s => s.UserId == userId && s.QuizId == quizId) // This will filter the scores to only the ones that have the same userId and quizId as the ones passed in
                .OrderByDescending(s => s.Points) // This will oder the scores in descending order
                .FirstOrDefault(); // We dont need to give this method a parameter since first in the list will be the highest score       

            if (existingHighScore == null || existingHighScore.Points < points)
            {
                var newHighScore = new Score
                {
                    UserId = userId,
                    QuizId = quizId,
                    Points = points,
                    PlayedAt = DateTime.Now
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
            return _context.Scores
                .Include(s => s.User)
                .Include(s => s.Quiz)
                .Where(s => s.QuizId == quizId)
                .OrderByDescending(s => s.Points)
                .ToList();
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

        public Score GetScoreForUserInQuiz(int userId, int quizId)
        {
            return _context.Scores
                .Include(s => s.User)
                .Include(s => s.Quiz)
                .FirstOrDefault(s => s.UserId == userId && s.QuizId == quizId);
        }

        public Score GetHighestScoreForUserInQuiz(int userId, int quizId)
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