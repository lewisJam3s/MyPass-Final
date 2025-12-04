using MyPass.Utilities;

namespace MyPass.Core.Entities
{
    public class SecureNoteItem : VaultItem
    {
        public string NoteEncrypted { get; set; } = string.Empty;

        // Computed property for UI
        public string? NoteContentDecrypted
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NoteEncrypted))
                    return null;

                return EncryptionHelper.Decrypt(NoteEncrypted);
            }
        }
    }
}

