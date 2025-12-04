namespace MyPass.Core.Entities
{
    public class User
    {
        // Primary Key
        public int Id { get; set; }
     
        public string Email { get; set; } = default!;
        public string MasterPasswordHash { get; set; } = default!;
     
        public ICollection<SecurityQuestion> SecurityQuestions { get; set; }
            = new List<SecurityQuestion>();
       
        public ICollection<VaultItem> VaultItems { get; set; }
            = new List<VaultItem>();
    }
}
