namespace MyPass.Core.Exceptions
{
    public class WeakPasswordException : Exception
    {
        public WeakPasswordException(string message) : base(message) { }
    }
}

