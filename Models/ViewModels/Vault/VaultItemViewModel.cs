namespace MyPass.Models.ViewModels.Vault
{
    public class VaultItemViewModel
    {
        public int Id { get; set; }
        public string ItemType { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string MaskedPreview { get; set; } = string.Empty;
    }
}
