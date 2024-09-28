using Library.Core.Entities;
using Library.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;
        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> Get(UserEntity user)
        {
            return await _context.Users.Include(x => x.Roles).FirstOrDefaultAsync(p => p.Email == user.Email && p.PasswordHash == user.PasswordHash);
        }
        public async Task<UserEntity?> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<UserEntity> GetInfo(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.Books)
                .FirstAsync(x => x.Id == id);
        }
        public async Task<RoleEntity?> GetRole(Role role)
        {
            var roleEntity = await _context.Roles.SingleOrDefaultAsync(r => r.Id == (int)role);
            return roleEntity;
        }
        public async Task Register(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
