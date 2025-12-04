using System.Diagnostics;

namespace MyPass.Utilities
{
    public static class DebugLogger
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[DEBUG] {message}");
            Debug.WriteLine($"[DEBUG] {message}");
        }

        public static void LogForm(IFormCollection form)
        {
            Console.WriteLine("=== RAW FORM ===");
            foreach (var field in form)
                Console.WriteLine($"{field.Key} = '{field.Value}'");
            Console.WriteLine("================");
        }
    }
}
