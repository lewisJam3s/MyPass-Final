namespace MyPass.Models.ViewModels.Vault
{
    public class CreditCardViewModel
    {
        public int Id { get; set; }
        public string CardHolder { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
    }

}
