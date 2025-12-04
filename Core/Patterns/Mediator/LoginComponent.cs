namespace MyPass.Core.Patterns.Mediator
{
    public class LoginComponent : IUiComponent
    {
        private IUiMediator? _mediator;

        public void SetMediator(IUiMediator mediator)
        {
            _mediator = mediator;
        }

        public void Receive(string message)
        {
            // Placeholder for actual UI interactions
            Console.WriteLine($"[LoginComponent] Received: {message}");
        }

        public void UserLoggedIn()
        {
            _mediator?.Send("User logged in", "LoginComponent");
        }
    }
}
