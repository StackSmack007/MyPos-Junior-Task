using System;

namespace CommonLibrary
{
    public static class Helpers
    {
        public static string ShortenText(int maxLength, string txt, string suffix = "...")
        {
            if (String.IsNullOrEmpty(txt))
            {
                return string.Empty;
            }

            if (txt.Length < maxLength)
            {
                return txt;
            }

            return txt.Substring(0, maxLength - suffix.Length) + suffix;
        }

        public static string SanitizePhone(string unformatedPhone)
        {
            var number = unformatedPhone.Replace(" ", "").Replace("-", "");
            return number.StartsWith("+") ? number : "+" + number;
        }
    }
}
