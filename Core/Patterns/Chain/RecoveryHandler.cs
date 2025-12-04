namespace MyPass.Core.Patterns.Chain
{
    public abstract class RecoveryHandler
    {
        protected RecoveryHandler? Next { get; private set; }

        public RecoveryHandler SetNext(RecoveryHandler next)
        {
            Next = next;
            return next;
        }

        public abstract bool Handle(RecoveryContext context);
    }
}

