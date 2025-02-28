using System;
using System.Security.Cryptography;
using System.Text;
using QuizApp.Models;
using QuizApp.Repositories;

namespace QuizApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        // The constructor takes an IUserRepository as a parameter 
        // so that the UserService can interact with the database.
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool RegisterUser(string username, string password)
        {
            // Using the GetUserByUsername method from the IUserRepository to check if the username already exists.
            if (_userRepository.GetUserByUsername(username) != null)
            {
                return false; // If the username already exists, return false.  
            }

            var newUser = new User
            {
                Username = username,
                PasswordHash = HashPassword(password) 
            };

            _userRepository.Add(newUser); // Add the new user to the database.
            _userRepository.SaveChanges(); // Save the changes to the database.
            return true; // Return true if the user was successfully registered.
        }

        public User AuthenticateUser(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username); 
           
            if (user == null || user.PasswordHash != HashPassword(password))
            {
                return null; // If the authentication fails, return null.
            }

            return user; // If the authentication is successful, return the user.
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id); // Get the user by id using the GetById method from the IUserRepository.            
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create()) // Create an instance of the SHA256 hashing algorithm.
            {   
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password)); // This will hash the password.
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower(); // Convert the hashed password to a string and return it. 
            }
        }
    }
}