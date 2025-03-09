using QuizApp.Services;

namespace QuizApp.Commands
{
    public class PlayQuizCommand : ICommand
    {
        private readonly IQuizService _quizService; 
        private readonly IScoreService _scoreService;
        private readonly ISessionService _sessionService;

        public PlayQuizCommand(
            IQuizService quizService,
            IScoreService scoreService,
            ISessionService sessionService)
        {
            _quizService = quizService;
            _scoreService = scoreService;
            _sessionService = sessionService;
        }    

        public string Name => "play";
        public string Description => "To view available quizzes to play type: quizzes";
        public string Category => "Commands for Quiz";

        public void Execute(string[] args)
        {

            if (!_sessionService.IsLoggedIn())
            {
                Console.WriteLine("You must be logged in to play a quiz. To login type: login");
                return;
            }

            // Validate arguments
            if (args.Length != 1 || !int.TryParse(args[0], out int quizId))
            {
                Console.WriteLine("To play a quiz type: play <quiz_id>");
                Console.WriteLine(Description);
                return;
            }

            var quiz = _quizService.GetQuizById(quizId);
            if (quiz == null)
            {                          
                Console.WriteLine($"Quiz with Id {quizId} not found.");
                return;
            }

            if (!quiz.Questions.Any())
            {
                Console.WriteLine($"Quiz '{quiz.Title}' has no questions.");
                return;
            }

            // Start the quiz
            Console.Clear();
            Console.WriteLine($"\n===== Starting Quiz: {quiz.Title} =====");
            Console.WriteLine($"Description: {quiz.Description}");
            Console.WriteLine($"Created by: {quiz.Creator?.Username}");
            Console.WriteLine($"Nr of Questions: {quiz.Questions.Count}");
            Console.WriteLine($"\nPress Enter to start...");
            Console.ReadLine();

            int score = 0;
            int questionNumber = 1;

            foreach (var question in quiz.Questions)
            {
                // Display Question
                Console.Clear();
                Console.WriteLine($"Question {questionNumber} of {quiz.Questions.Count} ");
                Console.WriteLine($"\n{question.Text}");

                // Display Options
                for (int i = 0; i < question.Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Options[i].Text}");
                }

                // Get users Answer
                int userAnswer = 0;
                bool validAnswer = false;
                
                // Validate users input 
                while (!validAnswer)
                {
                    Console.WriteLine($"\nYour answer (1-{question.Options.Count()}): ");
                    string? input = Console.ReadLine();

                    if (int.TryParse(input, out userAnswer) &&
                    userAnswer >= 1 && userAnswer <= question.Options.Count)
                    {
                        validAnswer = true;
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a number between 1 and {question.Options.Count}");
                    }
                }

                // Checks if answer is correct
                // Takes input minus 1 to get the index of the option, checks what value (true/false) IsCorrect has and saves it in a local isCorrect variable
                bool isCorrect = question.Options[userAnswer - 1].IsCorrect;
                if (isCorrect)
                {
                    score++;
                }

                Console.WriteLine($"\nPress Enter to continue...");
                Console.ReadLine();
                questionNumber++;
            }

            // Quiz Completed
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n===== Quiz Completed! =====");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You scored {score} out of {quiz.Questions.Count()}");

            // Save score to the user
            var currentUser = _sessionService.GetCurrentUser();
            if (currentUser == null)
            {
                Console.WriteLine("No user is currently logged in.");
                return;
            }

            int userId = currentUser.Id;
            var savedScore = _scoreService.SaveScore(userId, quizId, score);

            // Check if its a new high score
            var personalBest = _scoreService.GetHighestScoreForUserInQuiz(userId, quizId);
            if (personalBest != null && personalBest.Id == savedScore.Id)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nNew Personal Best!");
            }

            // Get leaderboard for the quiz 
            Console.WriteLine($"\nLeaderboard for quiz {quiz.Title} :");
            var leaderboard = _scoreService.GetLeaderboardForOneQuiz(quizId).Take(5);

            Console.WriteLine($"{"Rank",-5}{"Player",-15}{"Score",-10}{"Date",-20}");
            Console.WriteLine(new string('-', 50));

            int rank = 1;
            foreach (var entry in leaderboard)
            {
                string highlight = entry.UserId == userId ? " <- You" : "";
                Console.WriteLine($"{rank,-5}{entry.User?.Username,-15}{entry.Points,-10}{entry.PlayedAt.ToString("yyyy-MM-dd HH:mm"),-20}{highlight}");
                rank++;
            }
        }
    }
}