using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Repositories
{
    // This class will inherit from the generic Repository class
    // It will also implement the IUserRepository interface (which inherits from the IRepository interface)
    public class UserRepository : Repository<User>, IUserRepository
    {
        // This is the constructor that takes in QuizDbContext as a parameter
        // ': base(context)' will send the context to the base class Repository<User> 
        // otherwise, the base class will not have access to the context
        public UserRepository(QuizDbContext context) : base(context)
        {
        }

        // This is the method that IUserRepository requires to be implemented
        // It uses the FirstOrDefault method from LINQ, it will return the first user that matches the given username or null if no user is found
        // 'u => u.Username == username' is a lambda expression (arrow function) 
        // it sends the statement to the FirstOrDefault method
        // can be translated to: "for each user (u), check if the user's username (u.Username) is equal to the given username (username)"
        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}