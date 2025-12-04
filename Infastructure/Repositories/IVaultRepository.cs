using MyPass.Core.Entities;

namespace MyPass.Infrastructure.Repositories
{
    public interface IVaultRepository
    {
        Task<List<VaultItem>> GetUserVaultAsync(int userId);
        Task<VaultItem?> GetByIdAsync(int id, int userId); 
        Task AddAsync(VaultItem item);
        Task UpdateAsync(VaultItem item);
        Task DeleteAsync(VaultItem item);
        Task SaveChangesAsync();
    }
}


