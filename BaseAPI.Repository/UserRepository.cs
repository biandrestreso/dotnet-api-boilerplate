using BaseAPI.Entities.Models;
using BaseAPI.Entities;
using BaseAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseAPI.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public User? GetUserById(Guid userId)
        {
            return FindByCondition(user => user.Id.Equals(userId)).FirstOrDefault();
        }

        public User? GetUserByEmail(string email)
        {
            return FindByCondition(user => user.Email != null && user.Email.Equals(email)).FirstOrDefault();
        }

        public void CreateUser(User user)
        {
            Create(user);
        }

        public void UpdateUser(User? user)
        {
            if (user != null)
                Update(user);
            else 
                throw new DbUpdateConcurrencyException();
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public int GetUserCount()
        {
            return FindAll().Count();
        }
    }
}