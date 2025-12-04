namespace MyPass.Core.Entities
{
    public class IdentityItem : VaultItem
    {
        public string FullName { get; set; } = default!;
        public string PassportNumberEncrypted { get; set; } = string.Empty;
        public string LicenseNumberEncrypted { get; set; } = string.Empty;
        public string SocialSecurityNumberEncrypted { get; set; } = string.Empty;
    }
}
