using System.ComponentModel.DataAnnotations;

namespace MyPass.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Master password is required.")]
        public string MasterPassword { get; set; } = "";

        [Required(ErrorMessage = "Question 1 is required.")]
        public string Question1 { get; set; } = "";
        [Required(ErrorMessage = "Answer 1 is required.")]
        public string Answer1 { get; set; } = "";

        [Required(ErrorMessage = "Question 2 is required.")]
        public string Question2 { get; set; } = "";
        [Required(ErrorMessage = "Answer 2 is required.")]
        public string Answer2 { get; set; } = "";

        [Required(ErrorMessage = "Question 3 is required.")]
        public string Question3 { get; set; } = "";
        [Required(ErrorMessage = "Answer 3 is required.")]
        public string Answer3 { get; set; } = "";
    }
}




