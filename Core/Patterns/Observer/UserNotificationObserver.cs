namespace MyPass.Core.Patterns.Observer
{
    public class UserNotificationObserver : INotificationObserver
    {
        private readonly List<string> _messages = new();

        public IReadOnlyList<string> Messages => _messages;

        public void Update(string message)
        {
            _messages.Add(message);
        }
    }
}

