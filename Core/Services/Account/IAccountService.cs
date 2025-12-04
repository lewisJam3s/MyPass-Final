using MyPass.Core.Entities;

namespace MyPass.Core.Services.Account
{
    public interface IAccountService
    {
        Task<User?> LoginAsync(string email, string masterPassword);
        Task<User> RegisterAsync(
            string email,
            string masterPassword,
            List<(string question, string answer)> securityQuestions);
    }
}
