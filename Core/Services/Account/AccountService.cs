using MyPass.Core.Entities;
using MyPass.Core.Exceptions;
using MyPass.Core.Patterns.Observer;
using MyPass.Infrastructure.Repositories;
using MyPass.Utilities;

namespace MyPass.Core.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepo;
        private readonly PasswordStrengthMonitor _strengthMonitor;
        
        // ------ constructor ------
        public AccountService(IUserRepository userRepo, PasswordStrengthMonitor strengthMonitor)
        {
            _userRepo = userRepo;
            _strengthMonitor = strengthMonitor;
        }

        // ------ login method ------
        public async Task<User?> LoginAsync(string email, string masterPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(masterPassword))
                return null;

            var user = await _userRepo.GetByEmailAsync(email.Trim());
            if (user == null)
                return null;

            return HashingHelper.Verify(masterPassword, user.MasterPasswordHash)
                ? user
                : null;
        }
        
        // ------ registration method ------
        public async Task<User> RegisterAsync(
            string email,
            string masterPassword,
            List<(string question, string answer)> securityQuestions)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(masterPassword))
                throw new ArgumentException("Master password is required.", nameof(masterPassword));

            var normalizedEmail = email.Trim().ToLower();

            if (_strengthMonitor.IsWeak(masterPassword))
            {
               
                throw new WeakPasswordException("Your master password is too weak. Try adding upper-case letters, digits, or special symbols.");
            }
            // END WEAK PASSWORD CHECK

            // DUPLICATE EMAIL CHECK
            var existing = await _userRepo.GetByEmailAsync(normalizedEmail);
            if (existing != null)
            {
                // This will be caught in the controller and shown in the UI
                throw new InvalidOperationException("An account with this email already exists.");
            }
            // END DUPLICATE EMAIL CHECK
            var user = new User
            {
                Email = normalizedEmail,
                MasterPasswordHash = HashingHelper.Hash(masterPassword),
                
            };
            // Add security questions
            foreach (var (q, a) in securityQuestions)
            {
                user.SecurityQuestions.Add(new SecurityQuestion
                {
                    QuestionText = q,
                    AnswerHash = HashingHelper.Hash(a),
                });
            }
            // END ADD SECURITY QUESTIONS
            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();
            
            return user;
        }
    }
}

