namespace MyPass.Core.Patterns.Mediator
{

    // Mediator responsible for directing communication between UI components.
   
    public class UiMediator : IUiMediator
    {
        private readonly Dictionary<string, IUiComponent> _components = new();

        public void Register(string name, IUiComponent component)
        {
            if (!_components.ContainsKey(name))
            {
                _components[name] = component;
                component.SetMediator(this);
            }
        }

        public void Send(string message, string fromComponent)
        {
            foreach (var entry in _components)
            {
                if (entry.Key != fromComponent)
                {
                    entry.Value.Receive(message);
                }
            }
        }
    }
}
