using BaseAPI.Entities;
using BaseAPI.Entities.Models;

namespace BaseAPI.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User? GetUserById(Guid employeeId);
        User? GetUserByEmail(string email);
        void CreateUser(User employee);
        void UpdateUser(User? employee);
        void DeleteUser(User employee);
        int GetUserCount();
    }
}
