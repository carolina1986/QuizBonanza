using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class QuizDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;

        // Lägg till modeller här
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Score> Scores { get; set; }

        // Standard konstruktor behövs fortfarande för vissa operationer
        public QuizDbContext()
        {
        }

        // Konstruktor som tar en IConfiguration
        public QuizDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = string.Empty;
                
                // Om vi har en konfiguration, försök använda den
                if (_configuration != null)
                {
                    connectionString = _configuration.GetConnectionString("DefaultConnection") ??
                                      _configuration["DATABASE_URL"];
                }
                else
                {
                    // Fallback till miljövariabel direkt
                    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
                    Console.WriteLine($"Connection string from env var: '{connectionString}'");
                }
                
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigurera modellen här om det behövs
            base.OnModelCreating(modelBuilder);
        }
    }
}