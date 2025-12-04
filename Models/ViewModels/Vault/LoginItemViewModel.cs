namespace MyPass.Models.ViewModels.Vault
{
    public class LoginItemViewModel
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

}
