using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data 
{
    // ': DbContext' means that QuizDbContext inherits from DbContext
    public class QuizDbContext : DbContext
    {
        // 'DbSet<T>' is a collection of entities of type 'T' in the context
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet <Score> Scores { get; set; }

        // Configures the context to connect to the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection string for the database
            optionsBuilder.UseNpgsql("Host=localhost;Database=quizdb;Username=quizuser;Password=password123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between the Quiz and User
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Creator) // A quiz has one creator
                .WithMany(u => u.CreatedQuizzes) // A user can create many quizzes
                .HasForeignKey(q => q.CreatorId); // The foreign key is CreatorId

            // Configure the relationship between the Question and Quiz
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz) // A question has one quiz
                .WithMany(q => q.Questions) // A quiz can have many questions
                .HasForeignKey(q => q.QuizId); // The foreign key is QuizId    

            // Configure the relationship between the Option and Question
            modelBuilder.Entity<Option>() 
                .HasOne(o => o.Question) // An option has one question
                .WithMany(q => q.Options) // A question can have many options
                .HasForeignKey(o => o.QuestionId); // The foreign key is QuestionId    

            // Configure the relationship between the Score and User
            modelBuilder.Entity<Score>()
                .HasOne(s => s.User) // A score has one user
                .WithMany(u => u.Scores) // A user can have many scores
                .HasForeignKey(s => s.UserId); // The foreign key is UserId

            // Configure the relationship between the Score and Quiz
            modelBuilder.Entity<Score>()
                .HasOne(s => s.Quiz) // A score has one quiz
                .WithMany(q => q.Scores) // A quiz can have many scores
                .HasForeignKey(s => s.QuizId); // The foreign key is QuizId         
        }
    }
}