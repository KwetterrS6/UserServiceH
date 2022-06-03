using System.Collections.Generic;
using UserService.Models;

namespace UserService.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();

        List<User> GetAllUsers();
        User GetUserById(int id);
        User Login(string username, string password);
        void Register(User user);
        bool CheckUserExist(string email);
    }
}
