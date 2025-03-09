using QuizApp.Services;

namespace QuizApp.Commands
{
    public class RegisterCommand : ICommand
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public RegisterCommand(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        public string Name => "register"; // '=>' is short for get { return "register" }
        public string Description => "To Register a new user, enter: register <username> <password>";
        public string Category => "Commands for User";

        public void Execute(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("To Register a new user, enter: register <username> <password>");
                return;
            }

            string username = args[0];
            string password = args[1];

            bool success = _userService.RegisterUser(username, password);
            if (success)
            {
                Console.WriteLine($"User {username} has successfully been registered");

                var newUser = _userService.GetUserByUsername(username);
                if (newUser == null)
                {
                    Console.WriteLine("Erroe: User is null.");
                    return;
                }
                _sessionService.Login(newUser);
            }
            else
            {
                Console.WriteLine($"{username} has failed to register as a user, might be taken");
            }
        }
    }
}