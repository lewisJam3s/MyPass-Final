namespace MyPass.Core.Patterns.Proxy
{

    // Proxy controlling masked/unmasked access to sensitive data.

    public class SensitiveField : ISensitiveField
    {
        private readonly string _realValue;
        private bool _isMasked;

        public SensitiveField(string realValue, bool isMasked = true)
        {
            _realValue = realValue;
            _isMasked = isMasked;
        }

        public string GetDisplayValue()
        {
            if (_isMasked)
            {
                // Show at least 4 asterisks, but never reveal the structure.
                return new string('*', Math.Max(4, _realValue.Length));
            }

            return _realValue;
        }

        public string GetRealValue() => _realValue;

        public void Unmask() => _isMasked = false;

        public void Mask() => _isMasked = true;
    }
}
