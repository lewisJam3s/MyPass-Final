using System.Text;

namespace MyPass.Core.Patterns.Builder
{
    public class PasswordBuilder : IPasswordBuilder
    {
        private int _length = 12;
        private bool _upper = true;
        private bool _lower = true;
        private bool _digits = true;
        private bool _special = false;
        
        // ------ methods to configure the password ------
        public IPasswordBuilder WithLength(int length)
        {
            _length = length;
            return this;
        }

        public IPasswordBuilder IncludeUppercase(bool include)
        {
            _upper = include;
            return this;
        }

        public IPasswordBuilder IncludeLowercase(bool include)
        {
            _lower = include;
            return this;
        }

        public IPasswordBuilder IncludeDigits(bool include)
        {
            _digits = include;
            return this;
        }

        public IPasswordBuilder IncludeSpecial(bool include)
        {
            _special = include;
            return this;
        }
        
        
       // ------ method to build the password ------
        public string Build()
        {
            var chars = new List<char>();

            if (_upper) chars.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            if (_lower) chars.AddRange("abcdefghijklmnopqrstuvwxyz");
            if (_digits) chars.AddRange("0123456789");
            if (_special) chars.AddRange("!@#$%^&*()_+-=[]{}<>?/");

            if (!chars.Any())
                throw new InvalidOperationException("No character sets selected.");

            var random = new Random();
            var sb = new StringBuilder();

            for (int i = 0; i < _length; i++)
            {
                var c = chars[random.Next(chars.Count)];
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
