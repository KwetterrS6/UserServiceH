using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Models;
using UserService.Data;

namespace UserService.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public void Register(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
        }

        public User Login(String email, String password)
        {
            return _context.Users.FirstOrDefault(p => p.Email == email && p.Password == password);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool CheckUserExist(string email)
        {
            if (_context.Users.FirstOrDefault(p => p.Email == email) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}