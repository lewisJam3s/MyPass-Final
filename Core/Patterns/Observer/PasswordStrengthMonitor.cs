using System.Linq;

namespace MyPass.Core.Patterns.Observer
{
    public class PasswordStrengthMonitor
    {
        private readonly NotificationCenter _center;

        public PasswordStrengthMonitor(NotificationCenter center)
        {
            _center = center;
        }

        public bool IsWeak(string password)
        {
            int score = 0;

            if (password.Length >= 12) score++;
            if (password.Any(char.IsUpper)) score++;
            if (password.Any(char.IsLower)) score++;
            if (password.Any(char.IsDigit)) score++;
            if (password.Any(ch => "!@#$%^&*()_+-=[]{}<>?/".Contains(ch))) score++;

            return score <= 2;
        }
    }
}


