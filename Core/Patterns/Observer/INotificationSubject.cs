namespace MyPass.Core.Patterns.Observer
{
    public interface INotificationSubject
    {
        void Attach(INotificationObserver observer);
        void Detach(INotificationObserver observer);
        void Notify(string message);
    }
}
