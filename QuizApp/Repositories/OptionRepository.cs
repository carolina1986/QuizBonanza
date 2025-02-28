using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using QuizApp.Data;
using System.Runtime.CompilerServices;

namespace QuizApp.Repositories
{
    public class OptionRepository : Repository<Option>, IOptionRepository
    {
        public OptionRepository(QuizDbContext context) : base(context)
        {
        }

        public IEnumerable<Option> GetOptionsByQuestionId(int questionId)
        {
            return _context.Options
                .Where(o => o.QuestionId == questionId) // "For every option check if the questionId is equal to the questionId passed in"
                .ToList(); // "Return the list of options that have the same questionId"
        }

        public Option GetCorrectOptionByQuestionId(int questionId)
        {
            return _context.Options
                .Where(o => o.QuestionId == questionId && o.IsCorrect == true)
                .FirstOrDefault(o => o.QuestionId == questionId && o.IsCorrect == true);
        }

        public Option GetOptionByOptionId(int optionId)
        {
            return _context.Options
                .FirstOrDefault(o => o.Id == optionId);
        }
    }
}