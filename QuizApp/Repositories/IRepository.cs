// This interface is used to define the generic repository pattern.
// It contains the basic CRUD operations that are used in the repository pattern.
// The repository pattern is used to separate the data access logic from the business logic.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QuizApp.Repositories
{

    // 'where T : class' constrains the generic type T to reference (class) types only 
    public interface IRepository<T> where T : class
    {
        // This method will return the entity with the given id
        T GetById(int id);

        // IEnumarable is used so we can have a collection of entities (e.g. a list of users)
        IEnumerable<T> GetAll(); 

        // This method will find all the entities that match the given expression (e.g. all users with a specific name)
        IEnumerable<T> Find(Expression<Func<T, bool>> expression); 
        
        // 'T entity' is the entity that we want to add to the database
        void Add(T entity);

        // Using IEnumerable so we can add multiple entities to the database (e.g. a list of questions)
        void AddRange(IEnumerable<T> entities);

        void Update(T entity); 

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        void SaveChanges();
    }
}