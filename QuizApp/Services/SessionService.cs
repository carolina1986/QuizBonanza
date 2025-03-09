using QuizApp.Models;

namespace QuizApp.Services
{
    public interface ISessionService
    {
        void Login(User user);
        void Logout();
        bool IsLoggedIn();
        User? GetCurrentUser();
    }

    public class SessionService : ISessionService
    {
        private User? _currentUser;

        public void Login(User user)
        {
            _currentUser = user; 
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public bool IsLoggedIn()
        {
            return _currentUser != null;
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }
    }
}


