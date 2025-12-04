namespace MyPass.Models.ViewModels.Vault
{
    public class IdentityItemViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;
    }


}
