namespace MyPass.Core.Patterns.Mediator
{
    public class NotificationComponent : IUiComponent
    {
        private IUiMediator? _mediator;

        public void SetMediator(IUiMediator mediator)
        {
            _mediator = mediator;
        }

        public void Receive(string message)
        {
            Console.WriteLine($"[NotificationComponent] Received: {message}");
        }

        public void ShowNotification(string text)
        {
            _mediator?.Send($"Notification: {text}", "NotificationComponent");
        }
    }
}
