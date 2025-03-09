using QuizApp.Services;

namespace QuizApp.Commands
{
    public class ListQuizzesCommand : ICommand
    {
        private readonly IQuizService _quizService;

        public ListQuizzesCommand(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public string Name => "quizzes";
        public string Description => "To play a quiz, type: play <ID>";
        public string Category => "Commands for Quiz";

        public void Execute(string[] args)
        {
            var quizzes = _quizService.GetAllQuizzes();
            
            if (!quizzes.Any())
            {
                Console.WriteLine("There is no quizzes available. To create a quiz input: createquiz");
                return;    
            }

            Console.WriteLine("\n===== Available Quizzes =====");
            Console.WriteLine($"{"ID",-4} | {"Title",-30} | {"Description",-40} | {"Created By",-15} | {"Questions",-5}");
            Console.WriteLine(new string('-', 100)); // This will print the '-' character 100 times
            
            foreach (var quiz in quizzes)
            {
                string truncatedDescription = quiz.Description.Length > 37 
                    ? quiz.Description.Substring(0, 37) + "..." 
                    : quiz.Description;

                Console.WriteLine(
                    $"{quiz.Id,-4} | " +
                    $"{quiz.Title,-30} | " +
                    $"{truncatedDescription,-40} | " +
                    $"{quiz.Creator?.Username,-15} | " +
                    $"{quiz.Questions.Count,-5}"
                );
            }
        }
    }
}