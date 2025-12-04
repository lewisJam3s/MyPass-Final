namespace MyPass.Models.ViewModels.Account
{
    // Step 1: user just enters email
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; } = string.Empty;
    }

    // Step 2: show questions, get answers + new password
    public class VerifySecurityQuestionsViewModel
    {
        public string Email { get; set; } = string.Empty;

        public string Question1 { get; set; } = string.Empty;
        public string Question2 { get; set; } = string.Empty;
        public string Question3 { get; set; } = string.Empty;

        public string Answer1 { get; set; } = string.Empty;
        public string Answer2 { get; set; } = string.Empty;
        public string Answer3 { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;

    }
}

