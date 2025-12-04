namespace MyPass.Core.Entities
{
    public abstract class VaultItem
    {
        // Primary Key
        public int Id { get; set; }
        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        // Common Properties
        public string Name { get; set; } = default!;
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
