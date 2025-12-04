namespace MyPass.Core.Entities
{
    public class CreditCardItem : VaultItem
    {
        public string CardHolderName { get; set; } = default!;
        public string CardNumberEncrypted { get; set; } = default!;
        public string CvvEncrypted { get; set; } = default!;

        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
    }
}
