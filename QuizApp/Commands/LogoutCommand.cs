using QuizApp.Services;

namespace QuizApp.Commands
{

    public class LogoutCommand : ICommand
    {
        private readonly ISessionService _sessionService;

        public LogoutCommand(ISessionService sessionService)
        {
            _sessionService = sessionService;
        } 

        public string Name => "logout";
        public string Description => "To log out of your account type: logout";
        public string Category => "Commands for User";

        public void Execute(string[] args)
        {
            if (!_sessionService.IsLoggedIn())
            {
                Console.WriteLine("You are not logged in");
                return;
            }

            var currentUser = _sessionService.GetCurrentUser();
            _sessionService.Logout();
            Console.WriteLine($"Thanks for playing, {currentUser?.Username ?? "unknown user"}! \nYou have been logged out.");
        }
    }
}