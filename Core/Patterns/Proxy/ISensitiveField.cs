namespace MyPass.Core.Patterns.Proxy
{
    public interface ISensitiveField
    {
        string GetDisplayValue();   // what UI sees (masked or unmasked)
        string GetRealValue();      // the raw decrypted value
        void Unmask();
        void Mask();
    }
}
