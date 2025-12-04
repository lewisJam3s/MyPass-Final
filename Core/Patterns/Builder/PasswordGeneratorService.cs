namespace MyPass.Core.Patterns.Builder
{
    public class PasswordGeneratorService
    {
        private readonly IPasswordBuilder _builder;
        
        public PasswordGeneratorService(IPasswordBuilder builder)
        {
            _builder = builder;
        }
        // ------ method to generate a strong password ------
        public string GenerateStrongPassword()
        {
            return _builder
                .WithLength(16)
                .IncludeUppercase(true)
                .IncludeLowercase(true)
                .IncludeDigits(true)
                .IncludeSpecial(true)
                .Build();
        }
    }
}

