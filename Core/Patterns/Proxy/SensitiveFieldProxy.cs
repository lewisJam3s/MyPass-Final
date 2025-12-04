using MyPass.Utilities;

public class SensitiveFieldProxy
{
    private readonly string _encryptedValue;

    public SensitiveFieldProxy(string encryptedValue)
    {
        _encryptedValue = encryptedValue;
    }

    public string GetMasked()
    {
        return "••••••••••••••••••••••";
    }

    public string GetUnmasked()
    {
        return EncryptionHelper.Decrypt(_encryptedValue);
    }

    // Optional: unify interface
    public string GetValue(bool masked = true)
    {
        return masked ? GetMasked() : GetUnmasked();
    }
}

