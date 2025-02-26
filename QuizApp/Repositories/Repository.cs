// This class works as a middle man between the database and the rest of the application
// so that the application does not have to interact with the database directly.
// This is where the CRUD operations are implemented.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;

namespace QuizApp.Repositories
{
    // This class is a generic class that can be used to perform CRUD operations on any entity
    // that is passed to it. It implements the IRepository interface
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly QuizDbContext _context;
        private DbSet<T> _dbSet; // DbSet is collection of entities (e.g. DbSet<User> is a collection of User entities)

        // The constructor takes in QuizDbContext as a parameter
        public Repository(QuizDbContext context)
        {
            _context = context; // _context is the whole database (e.g. _context = new QuizDbContext() is the whole database)
            _dbSet = context.Set<T>(); // _dbSet is a specific table in the database (e.g. _dbSet = context.Set<User>() is the User table)
        }
        
        // The following methods are the implementation of the IRepository interface methods
        // This first method will return a single entity with the given id (e.g. a single user with the given id)
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        // This method will return all the entities in the table (e.g. all the users in the User table)
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        // This method will return all the entities that match the given expression (e.g. all users with a specific name, all users over 18 years old)
        // Expression<Func<T, bool>> is a lambda expression that takes an entity of type T and returns a boolean
        // (e.g. user => user.Name == "John" is a lambda expression that takes a user entity and returns true if the user's name is "John")
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression) //
        {
            return _dbSet.Where(expression);
        }

        // This method will add the given entity to the table (e.g. add a new user to the User table)
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        // This method will add multiple entities to the table (e.g. add a list of questions to the Question table)
        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        // This method will update the given entity in the table (e.g. update the user with the given id in the User table)
        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        // This method will remove the given entity from the table (e.g. remove the user with the given id from the User table)
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        // This method will remove multiple entities from the table (e.g. remove a list of questions from the Question table)
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // This method will save the changes made to the database
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}