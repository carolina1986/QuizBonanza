using QuizApp.Models;

namespace QuizApp.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        // This method (only signature) is here because it is specific to the User entity
        User? GetUserByUsername(string username);
    }
}