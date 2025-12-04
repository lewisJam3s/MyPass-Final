namespace MyPass.Core.Entities
{
    public class LoginItem : VaultItem
    {
        public string Username { get; set; } = default!;
        public string PasswordEncrypted { get; set; } = default!;
        public string Url { get; set; } = string.Empty;
    }
}
