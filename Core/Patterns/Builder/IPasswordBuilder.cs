namespace MyPass.Core.Patterns.Builder
{
    public interface IPasswordBuilder
    {
        // ------ methods to configure the password ------
        IPasswordBuilder WithLength(int length);
        IPasswordBuilder IncludeUppercase(bool include);
        IPasswordBuilder IncludeLowercase(bool include);
        IPasswordBuilder IncludeDigits(bool include);
        IPasswordBuilder IncludeSpecial(bool include);

        string Build();
    }
}
