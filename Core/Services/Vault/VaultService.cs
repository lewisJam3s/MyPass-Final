using MyPass.Core.Entities;
using MyPass.Infrastructure.Repositories;
using MyPass.Utilities;

namespace MyPass.Core.Services.Vault
{
    public class VaultService : IVaultService
    {
       
        private readonly IVaultRepository _vaultRepository;
        // ------ constructor ------
        public VaultService(IVaultRepository vaultRepository)
        {
            _vaultRepository = vaultRepository;
        }
        
        // ------ get all vault items for a user ------
        public Task<List<VaultItem>> GetVaultItemsForUserAsync(int userId)
        {
            return _vaultRepository.GetUserVaultAsync(userId);
        }

        public async Task<VaultItem?> GetItemByIdAsync(int id, int userId)
        {
            return await _vaultRepository.GetByIdAsync(id, userId);
        }
        
        // ------ login creation method ------
        public async Task<int> CreateLoginAsync(int userId, string name, string username, string passwordPlain, string url)
        {
            var item = new LoginItem
            {
                UserId = userId,
                Name = name,
                Username = username,
                PasswordEncrypted = EncryptionHelper.Encrypt(passwordPlain),
                Url = url
            };

            await _vaultRepository.AddAsync(item);
            await _vaultRepository.SaveChangesAsync();
            return item.Id;
        }
        
        // ------ credit card creation method ------
        public async Task<int> CreateCreditCardAsync(int userId, string name, string cardHolder, string cardNumberPlain, string cvvPlain, int expMonth, int expYear)
        {
            var item = new CreditCardItem
            {
                UserId = userId,
                Name = name,
                CardHolderName = cardHolder,
                CardNumberEncrypted = EncryptionHelper.Encrypt(cardNumberPlain),
                CvvEncrypted = EncryptionHelper.Encrypt(cvvPlain),
                ExpMonth = expMonth,
                ExpYear = expYear
            };

            await _vaultRepository.AddAsync(item);
            await _vaultRepository.SaveChangesAsync();
            return item.Id;
        }
        
        // ------ identity creation method ------
        public async Task<int> CreateIdentityAsync(int userId, string name, string fullName, string passportPlain, string licensePlain, string ssnPlain)
        {
            var item = new IdentityItem
            {
                UserId = userId,
                Name = name,
                FullName = fullName,
                PassportNumberEncrypted = EncryptionHelper.Encrypt(passportPlain),
                LicenseNumberEncrypted = EncryptionHelper.Encrypt(licensePlain),
                SocialSecurityNumberEncrypted = EncryptionHelper.Encrypt(ssnPlain)
            };

            await _vaultRepository.AddAsync(item);
            await _vaultRepository.SaveChangesAsync();
            return item.Id;
        }
        
        // ------ secure note creation method ------
        public async Task<int> CreateSecureNoteAsync(int userId, string title, string notePlain)
        {
            var item = new SecureNoteItem
            {
                UserId = userId,
                Name = title,
                NoteEncrypted = EncryptionHelper.Encrypt(notePlain)
            };

            await _vaultRepository.AddAsync(item);
            await _vaultRepository.SaveChangesAsync();
            return item.Id;
        }
       
        // ------ delete method ------
        public async Task DeleteItemAsync(int id, int userId)
        {
            var item = await _vaultRepository.GetByIdAsync(id, userId);
            if (item == null) return;

            await _vaultRepository.DeleteAsync(item);
            await _vaultRepository.SaveChangesAsync();
        }
        
        // ------ login update method ------
        public async Task UpdateLoginAsync(int id, int userId, string siteName, string username, string passwordPlain, string url)
        {
            var item = await _vaultRepository.GetByIdAsync(id, userId) as LoginItem;
            if (item == null) return;

            item.Name = siteName;
            item.Username = username;
            item.Url = url;

            if (!string.IsNullOrWhiteSpace(passwordPlain))
                item.PasswordEncrypted = EncryptionHelper.Encrypt(passwordPlain);

            await _vaultRepository.SaveChangesAsync();
        }
        
        // ------ credit card update method ------
        public async Task UpdateCreditCardAsync(int id, int userId, string cardHolder, string cardNumberPlain, string cvvPlain, int expMonth, int expYear)
        {
            var item = await _vaultRepository.GetByIdAsync(id, userId) as CreditCardItem;
            if (item == null) return;

            item.CardHolderName = cardHolder;
            item.ExpMonth = expMonth;
            item.ExpYear = expYear;

            if (!string.IsNullOrWhiteSpace(cardNumberPlain))
                item.CardNumberEncrypted = EncryptionHelper.Encrypt(cardNumberPlain);

            if (!string.IsNullOrWhiteSpace(cvvPlain))
                item.CvvEncrypted = EncryptionHelper.Encrypt(cvvPlain);

            await _vaultRepository.SaveChangesAsync();
        }
       
        // ------ identity update method ------
        public async Task UpdateIdentityAsync(int id, int userId, string fullName, string passportPlain, string licensePlain, string ssnPlain)
        {
            var item = await _vaultRepository.GetByIdAsync(id, userId) as IdentityItem;
            if (item == null) return;

            item.FullName = fullName;

            if (!string.IsNullOrWhiteSpace(passportPlain))
                item.PassportNumberEncrypted = EncryptionHelper.Encrypt(passportPlain);

            if (!string.IsNullOrWhiteSpace(licensePlain))
                item.LicenseNumberEncrypted = EncryptionHelper.Encrypt(licensePlain);

            if (!string.IsNullOrWhiteSpace(ssnPlain))
                item.SocialSecurityNumberEncrypted = EncryptionHelper.Encrypt(ssnPlain);

            await _vaultRepository.SaveChangesAsync();
        }
        
        // ------ secure note update method ------
        public async Task UpdateSecureNoteAsync(int id, int userId, string title, string contentPlain)
        {
            var item = await _vaultRepository.GetByIdAsync(id, userId) as SecureNoteItem;
            if (item == null) return;

            item.Name = title;

            if (!string.IsNullOrWhiteSpace(contentPlain))
                item.NoteEncrypted = EncryptionHelper.Encrypt(contentPlain);

            await _vaultRepository.SaveChangesAsync();
        }
    }
}


