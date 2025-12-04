namespace MyPass.Core.Entities
{
    public class SecurityQuestion
    {
        // Primary Key
        public int Id { get; set; }
        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        // Properties
        public string QuestionText { get; set; } = default!;
        public string AnswerHash { get; set; } = default!;
    }
}
