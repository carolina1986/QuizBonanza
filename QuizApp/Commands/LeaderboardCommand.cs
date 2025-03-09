using QuizApp.Services;

namespace QuizApp.Commands
{

    public class LeaderboardCommand : ICommand
    {
        private readonly IScoreService _scoreService;
        private readonly IQuizService _quizService;
        private readonly ISessionService _sessionService;
    

        public LeaderboardCommand(
            IScoreService scoreService,
            IQuizService quizService,
            ISessionService sessionService)
        {
            _scoreService = scoreService;
            _quizService = quizService;
            _sessionService = sessionService; 
        }

        public string Name => "leaderboard";
        public string Description => "View all options by typing; leaderboard";
        public string Category => "Commands for Leaderboard";

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("\nLeaderboard Options:");
                Console.WriteLine(" leaderboard me - To view your personal leaderboard");
                Console.WriteLine(" leaderboard <quiz_id> - To view leaderboard for specific quiz");
            return;
            }

            string option = args[0].ToLower();


            // User leaderboard
            if (option == "me")
            {
                if (!_sessionService.IsLoggedIn())
                {
                    Console.WriteLine("You must be logged in to view your scores. Type 'login' command first.");
                    return;
                }

                var currentUser = _sessionService.GetCurrentUser();
                if (currentUser == null)
                {
                    Console.WriteLine("Error: No user is logged in.");
                    return;
                }
                
                ShowPersonalScores(currentUser.Id);
            }

            // Quiz-specific leaderboard
            else if (int.TryParse(option, out int quizId))
            {
                var quiz = _quizService.GetQuizById(quizId);
                if (quiz == null)
                {
                    Console.WriteLine($"Quiz with ID {quizId} not found.");
                    return;
                }
                
                ShowQuizLeaderboard(quizId, quiz.Title);
            }
            else
            {
                Console.WriteLine("Invalid option. Type 'leaderboard' to see available options.");
            }
        }

        private void ShowQuizLeaderboard(int quizId, string quizTitle)
        {
            var scores = _scoreService.GetLeaderboardForOneQuiz(quizId);

            Console.WriteLine($"\n=== Leaderboard for: {quizTitle} ===");
            Console.WriteLine($"{"Rank",-5}{"Player",-15}{"Score",-10}{"Date",-20}");
            Console.WriteLine(new string('-', 50));

            int rank = 1;
            foreach (var score in scores)
            {
                if (score.User == null || score.Quiz == null)
                {
                    Console.WriteLine($"⚠️ DEBUG: User or Quiz is NULL for score {score.Points}");
                    continue; 
                }

                // Highlight current user if logged in
                string highlight = "";
                var currentUser = _sessionService.GetCurrentUser();
                if (_sessionService.IsLoggedIn() && currentUser != null && score.UserId == currentUser.Id)
                {
                    highlight = " <- You";
                }

                Console.WriteLine($"|{rank,-5}|{score.User.Username,-15}|{score.Points,-10}|{score.PlayedAt.ToString("yyyy-MM-dd HH:mm"),-20}|{highlight}");
                rank++;
            }
        }

        private void ShowPersonalScores(int userId)
        {
            var scores = _scoreService.GetLeaderboardForOneUser(userId);
            
            if (!scores.Any())
            {
                Console.WriteLine("\nYou haven't completed any quizzes yet. Type: 'play' command to play a quiz!");
                return;
            }
            
            Console.WriteLine("\n=== Your Scores ===");
            Console.WriteLine($"{"Quiz",-30}{"Score",-10}{"Date",-20}");
            Console.WriteLine(new string('-', 60));
            
            foreach (var score in scores)
            {
                if (score.Quiz != null)
                {
                    Console.WriteLine($"|{score.Quiz.Title,-30}|{score.Points,-10}|{score.PlayedAt.ToString("yyyy-MM-dd HH:mm"),-20}");
                }
                else 
                {
                    return;
                }
            }
        }
    }
}