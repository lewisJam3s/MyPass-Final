namespace MyPass.Core.Patterns.Observer
{
    public class NotificationCenter : INotificationSubject
    {
        private readonly List<INotificationObserver> _observers = new();

        public void Attach(INotificationObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(INotificationObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var obs in _observers)
                obs.Update(message);
        }
    }
}

