using System.Security;

namespace StateBuilder.Library.Security.Extensions
{
    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                SecureString result = new SecureString();
                foreach (char character in value)
                {
                    result.AppendChar(character);
                }
                return result;
            }
            return null;
        }
    }
}
