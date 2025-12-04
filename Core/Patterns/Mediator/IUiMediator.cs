namespace MyPass.Core.Patterns.Mediator
{
    public interface IUiMediator
    {
        void Register(string name, IUiComponent component);
        void Send(string message, string fromComponent);
    }
}
