using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Repository.Context;
using ProEventos.Repository.Interfaces;

namespace ProEventos.Repository.Repositories
{
    public class UserRepository : GeralRepository, IUserRepository
    {
        private readonly ProEventosContext _context;

        public UserRepository(ProEventosContext context): base(context)
        {
            _context = context;            
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}