using QuizApp.Models;

namespace QuizApp.Services
{
    public interface IUserService
    {
        bool RegisterUser(string username, string password);
        User? AuthenticateUser(string username, string password);
        User? GetUserById(int id);
        User? GetUserByUsername(string username);
        User UpdateUser(User user);
    }
}