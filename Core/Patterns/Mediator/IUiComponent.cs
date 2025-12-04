namespace MyPass.Core.Patterns.Mediator
{
    public interface IUiComponent
    {
        void SetMediator(IUiMediator mediator);
        void Receive(string message);
    }
}
