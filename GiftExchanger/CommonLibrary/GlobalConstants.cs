using System;
using System.Collections.Generic;

namespace CommonLibrary
{
    public static class GlobalConstants
    {
        private const string ERROR_PREFIX = "Error:";
        public static readonly decimal InitialRegisterCredit = 100m;
        public static readonly int AdminsCount = 1;
        public static IReadOnlyDictionary<string, string> RoleNames = new Dictionary<string, string>() { ["Administrator"] = "Admin" };

        public static Func<string, string> PhoneAlreadyUsedError = (x) => $"{ERROR_PREFIX} The phone number {x} is already set.";
        public static Func<string, string> PhoneAlreadySetError = (x) => $"{ERROR_PREFIX} The phone number {x} is already set.";

        public static string FormatPhoneString(string unformatedPhone)
        {
            var number = unformatedPhone.Replace(" ", "").Replace("-", "");
            return number.StartsWith("+") ? number : "+" + number;
        }
    }
}
