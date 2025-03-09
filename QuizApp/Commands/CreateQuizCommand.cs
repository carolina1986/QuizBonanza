using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Commands
{
    public class CreateQuizCommand : ICommand
    {
        private readonly IQuizService _quizService;
        private readonly ISessionService _sessionService;

        public CreateQuizCommand(IQuizService quizService, ISessionService sessionService)
        {
            _quizService = quizService;
            _sessionService = sessionService;
        }

        public string Name => "createquiz";
        public string Description => "To create a new quizz type: createquiz";
        public string Category => "Commands for Quiz";

        public void Execute(string[] args)
        {
            if (!_sessionService.IsLoggedIn())
            {
                Console.WriteLine("You must be logged in to create a quiz. \nTo login type: login \nTo regiter a new user type: register");
                return;
            }

            Console.WriteLine("\n===== Create a New Quiz =====");
            
            Console.Write("Enter quiz title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Enter quiz description: ");
            string description = Console.ReadLine() ?? "";

            var questions = new List<Question>();
            bool addingQuestions = true; 

            Console.WriteLine("\n=== Add Questions ===");
            Console.WriteLine("For each question, you'll need to provide 2 options.");
            Console.WriteLine("One option must be marked as correct.");

            while (addingQuestions)
            {
                Console.Write("\nEnter question: ");
                string? questionText = Console.ReadLine();

                var options = new List<Option>();
                bool validCorrectOption = false;
                int correctOptionIndex = 0;

                for (int i = 1; i <= 2; i++)
                {
                    Console.Write($"Enter option {i}: ");
                    string? optionText = Console.ReadLine();

                    options.Add(new Option {
                        Text = optionText ?? "",
                        IsCorrect = false
                    });
                }

                while (!validCorrectOption)
                {
                    Console.Write("Which option is correct (use index 1 - 2)? ");
                    string? correctInput = Console.ReadLine();
                    if (int.TryParse(correctInput, out correctOptionIndex) &&
                        correctOptionIndex >= 1 && correctOptionIndex <= 2)
                    {
                        validCorrectOption = true;
                        options[correctOptionIndex - 1].IsCorrect = true;
                    }                        
                    else
                    {
                        Console.WriteLine("Invalid option. ");
                        Console.Write("Which option is correct (use index 1 - 2)? ");
                    }
                }

                questions.Add(new Question {
                    Text = questionText ?? "",
                    Options = options
                });

                // Add another question
                Console.Write("\nAdd another question? (y/n)");
                string addAnother = Console.ReadLine()?.ToLower() ?? "n";

                if (addAnother != "y" && addAnother != "yes")
                {
                    addingQuestions = false;
                } 
            }               

            var currentUser = _sessionService.GetCurrentUser();
            if (currentUser == null)
            {
                Console.WriteLine("No user is logged in");
                return;
            }

            var userId = currentUser.Id;
            var quiz = _quizService.CreateQuiz(title, description, userId, questions);

            Console.WriteLine($"\nQuiz {title} has successfully been created with ID: {quiz.Id}");
            
        }
    }
}