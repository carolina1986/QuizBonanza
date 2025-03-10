using Microsoft.Extensions.Configuration; // To load and manage configurations from files and environment variables
using QuizApp.Commands; // To handle commands within the app
using QuizApp.Data; // To use the database model and context
using QuizApp.Repositories; // To manage database operations through repositories
using QuizApp.Services; // To provide various services like user, quiz, and score logic
using System.IO; // To read and write files, such as appsettings.json

namespace QuizApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Clear the console and display a message that the app is loading
            Console.Clear();
            Console.WriteLine("Loading...");

            // Configure the application to load environment variables and settings from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the current directory as the base path
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Load settings from appsettings.json
                .AddEnvironmentVariables() // Add environment variables to override settings
                .Build(); // Build the configuration

            // Retrieve the connection string from the configuration, or use an environment variable if not present
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                                  configuration["DATABASE_URL"];

            // Initialize the database
            using (var db = new QuizDbContext(configuration)) // Create an instance of the database context
            {
                Console.WriteLine("Setting up the database...");
                db.Database.EnsureCreated(); // Create the database if it doesn't already exist
            }

            // Set up dependency injection (DI) for the database context
            var context = new QuizDbContext(configuration);

            // Create instances of repositories to handle database access
            var userRepository = new UserRepository(context); // User repository
            var quizRepository = new QuizRepository(context); // Quiz repository
            var questionRepository = new QuestionRepository(context); // Question repository
            var optionRepository = new OptionRepository(context); // Option repository
            var scoreRepository = new ScoreRepository(context); // Score repository

            // Create instances of services that use repositories for business logic
            var userService = new UserService(userRepository); // User service
            var quizService = new QuizService(quizRepository, userRepository); // Quiz service
            var questionService = new QuestionService(questionRepository, optionRepository, quizRepository); // Question service
            var scoreService = new ScoreService(scoreRepository, quizRepository, userRepository); // Score service
            var sessionService = new SessionService(); // Session management service

            // Create an instance of the command handler to handle user commands
            var commandHandler = new CommandHandler();

            // Register various commands that the user can execute
            commandHandler.CommandRegistration(new RegisterCommand(userService, sessionService)); // Register user
            commandHandler.CommandRegistration(new PlayQuizCommand(quizService, scoreService, sessionService)); // Play quiz
            commandHandler.CommandRegistration(new LogoutCommand(sessionService)); // Logout user
            commandHandler.CommandRegistration(new LoginCommand(userService, sessionService)); // Login user
            commandHandler.CommandRegistration(new ListQuizzesCommand(quizService)); // List available quizzes
            commandHandler.CommandRegistration(new LeaderboardCommand(scoreService, quizService, sessionService)); // Show leaderboard
            commandHandler.CommandRegistration(new CreateQuizCommand(quizService, sessionService)); // Create a new quiz
            
            // Create a dictionary to hold the available commands
            var commandsDictionary = new Dictionary<string, ICommand>();
            commandHandler.CommandRegistration(new MenuCommand(commandsDictionary)); // Register menu command

            // Register all commands into the dictionary with their names in lowercase
            foreach (var cmd in commandHandler.GetAllCommands())
            {
                if (cmd.Name == null)
                {
                    Console.WriteLine("Error: command not valid");
                    return;
                }
                commandsDictionary[cmd.Name.ToLower()] = cmd; // Map command name to command object
            }

            // Welcome message and prompt for user input
            Console.WriteLine("\nWelcome to my humble Quiz App!");
            Console.WriteLine("Type 'menu' to see available commands.");

            // Main application loop
            bool running = true;
            while (running)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                // Check for invalid input or exit command
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Error: invalid input");
                }
                else if (input.ToLower() == "exit")
                {
                    running = false; // Exit the loop if the user types 'exit'
                }
                else
                {
                    // Process the command if the input is valid
                    commandHandler.HandleCommand(input);
                }
            }

            // Goodbye message when the user exits
            Console.WriteLine("Thank you for playing!");
            Console.Clear(); // Clear the console before exiting
        }
    }
}
