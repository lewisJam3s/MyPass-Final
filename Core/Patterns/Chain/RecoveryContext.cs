using MyPass.Core.Entities;

namespace MyPass.Core.Patterns.Chain
{
    public class RecoveryContext
    {
        public User User { get; set; } = default!;

        public string Answer1 { get; set; } = string.Empty;
        public string Answer2 { get; set; } = string.Empty;
        public string Answer3 { get; set; } = string.Empty;
    }
}
