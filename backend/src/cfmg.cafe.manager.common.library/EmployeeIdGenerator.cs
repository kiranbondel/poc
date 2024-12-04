namespace Cfmg.Cafe.Manager.Common.Library
{
    public static class EmployeeIdGenerator
    {
        private const string IdPrefix = "UI";
        private const int IdLength = 7;
        private const string AlphanumericChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string GenerateEmployeeId(string? lastEmployeeId)
        {
            string lastAlphanumericPart = lastEmployeeId?.Substring(IdPrefix.Length) ?? new string('0', IdLength);
            string nextAlphanumericPart = IncrementAlphanumeric(lastAlphanumericPart);
            string nextEmployeeId = $"{IdPrefix}{nextAlphanumericPart}";
            return nextEmployeeId;
        }
        
        private static string IncrementAlphanumeric(string current)
        {
            char[] chars = current.ToCharArray();
            int index = chars.Length - 1;

            while (index >= 0)
            {
                int currentCharIndex = AlphanumericChars.IndexOf(chars[index]);

                if (currentCharIndex < AlphanumericChars.Length - 1)
                {
                    chars[index] = AlphanumericChars[currentCharIndex + 1];
                    break;
                }
                else
                {
                    chars[index] = AlphanumericChars[0];
                    index--;
                }
            }

            return new string(chars);
        }
    }

}
