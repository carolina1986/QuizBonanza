using QuizApp.Services;

namespace QuizApp.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public LoginCommand(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        public string Name => "login";
        public string Description => "To log in to your account input: login <username> <password>";
        public string Category => "Commands for User";

        public void Execute(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("To login type: login <username> <password>");
                return;
            }

            if (_sessionService.IsLoggedIn())
            {
               var currentUser = _sessionService.GetCurrentUser(); 
               Console.WriteLine($"You are already logged in as {currentUser?.Username ?? "unknown user"}");
               Console.WriteLine($"Logout if you want to login as another user.");
               return; 
            }

            string username = args[0];
            string password = args[1];

            var user = _userService.AuthenticateUser(username, password);
            if (user != null)
            {
                _sessionService.Login(user);
                Console.Clear();
                Console.WriteLine($"Welcome {username}!");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("(type 'menu' to see available commands)");
            }
            else
            {
                Console.WriteLine("Invalid username or password");
            }
        }
    }
}