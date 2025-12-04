using BCrypt.Net;

namespace MyPass.Utilities
{
    public static class HashingHelper
    {
        public static string Hash(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Hashing empty input is not allowed. Check form binding — input was empty.", nameof(input));


            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public static bool Verify(string input, string hashed)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(hashed))
                return false;

            return BCrypt.Net.BCrypt.Verify(input, hashed);
        }
    }
}
