using Microsoft.EntityFrameworkCore;
using MyPass.Core.Entities;
using MyPass.Infrastructure.Data;

namespace MyPass.Infrastructure.Repositories
{
    public class VaultRepository : IVaultRepository
    {
        private readonly MyPassDbContext _context;

        public VaultRepository(MyPassDbContext ctx)
        {
            _context = ctx;
        }

        public Task<List<VaultItem>> GetUserVaultAsync(int userId)
        {
            return _context.VaultItems
                .Where(v => v.UserId == userId)
                .ToListAsync();
        }

        public Task<VaultItem?> GetByIdAsync(int id, int userId) 
        {
            return _context.VaultItems
                .FirstOrDefaultAsync(v => v.Id == id && v.UserId == userId);
        }

        public Task AddAsync(VaultItem item)
        {
            return _context.VaultItems.AddAsync(item).AsTask();
        }

        public Task UpdateAsync(VaultItem item)
        {
            _context.VaultItems.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(VaultItem item)
        {
            _context.VaultItems.Remove(item);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

