using Microsoft.EntityFrameworkCore;
using MyPass.Core.Entities;
using MyPass.Infrastructure.Data;

namespace MyPass.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyPassDbContext _context;

        public UserRepository(MyPassDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var normalized = email.Trim().ToLower();

            return _context.Users
                .Include(u => u.SecurityQuestions)
                .Include(u => u.VaultItems)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalized);
        }

        public Task<User?> GetByIdAsync(int id)
        {
            return _context.Users
                .Include(u => u.SecurityQuestions)
                .Include(u => u.VaultItems)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
