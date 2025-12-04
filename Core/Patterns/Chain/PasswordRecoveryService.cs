using MyPass.Core.Entities;
using MyPass.Infrastructure.Repositories;
using MyPass.Utilities;

namespace MyPass.Core.Services.Account
{

    // RECOVERY CONTEXT + HANDLERS
    // (Chain of Responsibility)
 
    public class RecoveryContext
    {
        public string Email { get; set; } = string.Empty;
        public User? User { get; set; }

        public string Answer1 { get; set; } = string.Empty;
        public string Answer2 { get; set; } = string.Empty;
        public string Answer3 { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public bool Success { get; set; }
    }

    public interface IRecoveryHandler
    {
        IRecoveryHandler SetNext(IRecoveryHandler next);
        Task HandleAsync(RecoveryContext context);
    }

    public abstract class RecoveryHandlerBase : IRecoveryHandler
    {
        protected IRecoveryHandler? _next;

        public IRecoveryHandler SetNext(IRecoveryHandler next)
        {
            _next = next;
            return next;
        }

        public abstract Task HandleAsync(RecoveryContext context);
    }

    // 1) Look up user by email
    public class EmailLookupHandler : RecoveryHandlerBase
    {
        private readonly IUserRepository _userRepo;

        public EmailLookupHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public override async Task HandleAsync(RecoveryContext context)
        {
            var normalized = context.Email.Trim().ToLower();
            var user = await _userRepo.GetByEmailAsync(normalized);

            if (user == null)
            {
                context.Success = false;
                return;
            }

            context.User = user;

            if (_next != null)
                await _next.HandleAsync(context);
        }
    }

    // 2) Check first security question
    public class Question1Handler : RecoveryHandlerBase
    {
        public override Task HandleAsync(RecoveryContext context)
        {
            if (context.User == null)
            {
                context.Success = false;
                return Task.CompletedTask;
            }

            var questions = context.User.SecurityQuestions
                .OrderBy(q => q.Id)
                .ToList();

            var q1 = questions.ElementAtOrDefault(0);
            if (q1 == null || !HashingHelper.Verify(context.Answer1, q1.AnswerHash))
            {
                context.Success = false;
                return Task.CompletedTask;
            }

            if (_next != null)
                return _next.HandleAsync(context);

            context.Success = true;
            return Task.CompletedTask;
        }
    }

    // 3) Check second question
    public class Question2Handler : RecoveryHandlerBase
    {
        public override Task HandleAsync(RecoveryContext context)
        {
            if (context.User == null)
            {
                context.Success = false;
                return Task.CompletedTask;
            }

            var questions = context.User.SecurityQuestions
                .OrderBy(q => q.Id)
                .ToList();

            var q2 = questions.ElementAtOrDefault(1);
            if (q2 == null || !HashingHelper.Verify(context.Answer2, q2.AnswerHash))
            {
                context.Success = false;
                return Task.CompletedTask;
            }

            if (_next != null)
                return _next.HandleAsync(context);

            context.Success = true;
            return Task.CompletedTask;
        }
    }

    // 4) Check third question
    public class Question3Handler : RecoveryHandlerBase
    {
        public override Task HandleAsync(RecoveryContext context)
        {
            if (context.User == null)
            {
                context.Success = false;
                return Task.CompletedTask;
            }

            var questions = context.User.SecurityQuestions
                .OrderBy(q => q.Id)
                .ToList();

            var q3 = questions.ElementAtOrDefault(2);
            if (q3 == null || !HashingHelper.Verify(context.Answer3, q3.AnswerHash))
            {
                context.Success = false;
                return Task.CompletedTask;
            }

            if (_next != null)
                return _next.HandleAsync(context);

            context.Success = true;
            return Task.CompletedTask;
        }
    }

    // 5) If all answers ok, reset password
    public class ResetPasswordHandler : RecoveryHandlerBase
    {
        private readonly IUserRepository _userRepo;

        public ResetPasswordHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public override async Task HandleAsync(RecoveryContext context)
        {
            if (context.User == null)
            {
                context.Success = false;
                return;
            }

            context.User.MasterPasswordHash = HashingHelper.Hash(context.NewPassword);
            await _userRepo.SaveChangesAsync();

            context.Success = true;

            if (_next != null)
                await _next.HandleAsync(context);
        }
    }

    // SERVICE used by controller

    public class PasswordRecoveryService
    {
        private readonly IUserRepository _userRepo;

        public PasswordRecoveryService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<string>?> GetQuestionsForEmailAsync(string email)
        {
            var normalized = email.Trim().ToLower();
            var user = await _userRepo.GetByEmailAsync(normalized);
            if (user == null)
                return null;

            var questions = user.SecurityQuestions
                .OrderBy(q => q.Id)
                .Take(3)
                .Select(q => q.QuestionText)
                .ToList();

            if (questions.Count < 3)
                return null;

            return questions;
        }

        public async Task<bool> TryRecoverAsync(
            string email,
            string answer1,
            string answer2,
            string answer3,
            string newPassword)
        {
            var context = new RecoveryContext
            {
                Email = email,
                Answer1 = answer1,
                Answer2 = answer2,
                Answer3 = answer3,
                NewPassword = newPassword
            };

            var emailHandler = new EmailLookupHandler(_userRepo);
            var q1 = new Question1Handler();
            var q2 = new Question2Handler();
            var q3 = new Question3Handler();
            var reset = new ResetPasswordHandler(_userRepo);

            // Chain: Email -> Q1 -> Q2 -> Q3 -> Reset
            emailHandler
                .SetNext(q1)
                .SetNext(q2)
                .SetNext(q3)
                .SetNext(reset);

            await emailHandler.HandleAsync(context);

            return context.Success;
        }
    }
}
