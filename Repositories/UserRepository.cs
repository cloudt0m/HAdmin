using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HAdmin.Data;
using HAdmin.Models;

namespace HAdmin.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid id);
        Task<User> GetUserByName(string name);
        Task<IEnumerable<User>> GetAll();
        Task Add(User user);
        Task Delete(Guid id);
        Task Update(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDataContext _context;
        public UserRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<User> Get(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByName(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            var userToUpdate = await _context.Users.FindAsync(user.Id);
            if (userToUpdate == null)
                throw new NullReferenceException();
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.IdNumber = user.IdNumber;
            userToUpdate.FullName = user.FullName;
            userToUpdate.Password = user.Password;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null)
                throw new NullReferenceException();
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }
    }
}