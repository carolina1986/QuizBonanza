// When running the application his code takes care of all the database operations
// It sets up the tables in the database and configures the relationships between the tables
// It also configures the connection to the database

using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data 
{
    public class QuizDbContext : DbContext // 'DbContext' is a class in Entity Framework Core that represents a session with the database
    {
        // Sets up the tables in the database
        // Each DbSet represents a table in the database
        // By using get; + set; we can read and write to the tables
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet <Score> Scores { get; set; }

        // Configures the connection to the database
        // using protected so that only classes that inherit from QuizDbContext can access this method
        // usin override so that we can override the method in the base class
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            optionsBuilder.UseNpgsql(connectionString);
        }

        // OnModelCreating is a method that is called when the database is being created
        // it allows us to configure the database and set up the relationships between the tables
        // Using protected so that only classes that inherit from QuizDbContext can access this method
        // Usin override so that we can override the method in the base class 
        // (DbContext class in this case, because we are inheriting from it and it has the OnModelCreating method)  
        // ModelBuilder is a EF class that configures the database and the relationships between the tables
        // modelBuilder is an instance of the ModelBuilder class
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