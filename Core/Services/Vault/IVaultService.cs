using MyPass.Core.Entities;

namespace MyPass.Core.Services.Vault
{
    public interface IVaultService
    {
        Task<List<VaultItem>> GetVaultItemsForUserAsync(int userId);

        Task<int> CreateLoginAsync(int userId, string name, string username, string passwordPlain, string url);
        Task<int> CreateCreditCardAsync(int userId, string name, string cardHolder, string cardNumberPlain, string cvvPlain, int expMonth, int expYear);
        Task<int> CreateIdentityAsync(int userId, string name, string fullName, string passportPlain, string licensePlain, string ssnPlain);
        Task<int> CreateSecureNoteAsync(int userId, string title, string notePlain);
        Task UpdateLoginAsync(int id, int userId, string siteName, string username, string password, string url);
        Task UpdateCreditCardAsync(int id, int userId, string holder, string number, string cvv, int month, int year);
        Task UpdateIdentityAsync(int id, int userId, string fullName, string passport, string license, string ssn);
        Task UpdateSecureNoteAsync(int id, int userId, string title, string content);

        Task<VaultItem?> GetItemByIdAsync(int id, int userId);
        Task DeleteItemAsync(int id, int userId);
      
    }
}
