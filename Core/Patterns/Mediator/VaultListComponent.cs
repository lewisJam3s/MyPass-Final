namespace MyPass.Core.Patterns.Mediator
{
    public class VaultListComponent : IUiComponent
    {
        private IUiMediator? _mediator;

        public void SetMediator(IUiMediator mediator)
        {
            _mediator = mediator;
        }

        public void Receive(string message)
        {
            Console.WriteLine($"[VaultListComponent] Received: {message}");
        }

        public void RefreshVault()
        {
            _mediator?.Send("Vault refreshed", "VaultListComponent");
        }
    }
}
