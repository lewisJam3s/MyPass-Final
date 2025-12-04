using Microsoft.EntityFrameworkCore;
using MyPass.Core.Entities;

namespace MyPass.Infrastructure.Data
{
    public class MyPassDbContext : DbContext
    {
        public MyPassDbContext(DbContextOptions<MyPassDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<SecurityQuestion> SecurityQuestions => Set<SecurityQuestion>();
        public DbSet<VaultItem> VaultItems => Set<VaultItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VaultItem>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<LoginItem>("Login")
                .HasValue<CreditCardItem>("CreditCard")
                .HasValue<IdentityItem>("Identity")
                .HasValue<SecureNoteItem>("SecureNote");
        }
    }
}
