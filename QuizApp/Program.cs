using QuizApp.Commands;
using QuizApp.Data;
using QuizApp.Repositories;
using QuizApp.Services;

namespace QuizApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Loading...");

            // Initialize database
            using (var db = new QuizDbContext())
            {
                Console.WriteLine("Setting up the database...");
                db.Database.EnsureCreated();
            }

            // Set up dependency injection
            var context = new QuizDbContext();

            // Repositories
            var userRepository = new UserRepository(context);
            var quizRepository = new QuizRepository(context);
            var questionRepository = new QuestionRepository(context);
            var optionRepository = new OptionRepository(context);
            var scoreRepository = new ScoreRepository(context);


            // Services
            var userService = new UserService(userRepository);
            var quizService = new QuizService(quizRepository, userRepository);
            var questionService = new QuestionService(questionRepository, optionRepository, quizRepository);
            var scoreService = new ScoreService(scoreRepository, quizRepository, userRepository);
            var sessionService = new SessionService();
            

            // Command handler
            var commandHandler = new CommandHandler();
            commandHandler.CommandRegistration(new RegisterCommand(userService, sessionService));
            commandHandler.CommandRegistration(new PlayQuizCommand(quizService, scoreService, sessionService));
            commandHandler.CommandRegistration(new LogoutCommand(sessionService));
            commandHandler.CommandRegistration(new LoginCommand(userService, sessionService));
            commandHandler.CommandRegistration(new ListQuizzesCommand(quizService));
            commandHandler.CommandRegistration(new LeaderboardCommand(scoreService, quizService, sessionService));
            commandHandler.CommandRegistration(new CreateQuizCommand(quizService, sessionService));
            
            var commandsDictionary = new Dictionary<string, ICommand>();
            commandHandler.CommandRegistration(new MenuCommand(commandsDictionary));
            foreach (var cmd in commandHandler.GetAllCommands())
            {
                if (cmd.Name == null)
                {
                    Console.WriteLine("Error: command not valid");
                    return;
                }
                commandsDictionary[cmd.Name.ToLower()] = cmd;
            }
            
            Console.WriteLine("\nWelcome to my humble Quiz App!");
            Console.WriteLine("Type 'menu' to see available commands.");

            // Main application loop
            bool running = true;
            while (running)
            {
                
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Error: invalid input");
                }
                else if (input.ToLower() == "exit")
                {
                    running = false;
                }
                else
                {
                    commandHandler.HandleCommand(input);
                }
            }

            Console.WriteLine("Thank you for playing!");
            Console.Clear();
        }
    }
}