using System;
using System.Collections.Generic;
using QuizApp.Models;
using QuizApp.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Repositories
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public QuizRepository(QuizDbContext context ) : base(context)
        {
        }

        public Quiz GetQuizWithQuestions(int quizId)
        {
            return _context.Quizzes // This will return all the quizzes in the database
                .Include(q => q.Questions) // This will include the questions for each quiz
                .FirstOrDefault(q => q.Id == quizId); // This will return the quiz with the given id
        }

        public Quiz GetQuizWithQuestionsAndOptions(int quizId)
        {
            return _context.Quizzes 
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options) // This will include the options for each question
                .FirstOrDefault(q => q.Id == quizId);
        }

        public IEnumerable<Quiz> GetQuizzesByCreator(int creatorId)
        {
            return _context.Quizzes
                .Where(q => q.CreatorId == creatorId)
                .ToList();
        }
    }
}
