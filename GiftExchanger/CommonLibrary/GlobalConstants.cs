using System;

namespace CommonLibrary
{
    public static class GlobalConstants
    {
        private const string ERROR_PREFIX = "Error:";
        public static readonly decimal InitialRegisterCredit = 100m;
        public static readonly int AdminsCount = 1;
        public const string AdministratorRole = "Admin";

        public static Func<string, string> GeneralError = (er) => $"{ERROR_PREFIX} {er}";
        public static Func<string, string> PhoneAlreadyUsedError = (x) => $"{ERROR_PREFIX} The phone number {x} is already set.";
        public static Func<string, string> PhoneAlreadySetError = (x) => $"{ERROR_PREFIX} The phone number {x} is already set.";
        public static Func<string, string> UserNotLocatedByPhoneOrUserNameError = (x) => $"{ERROR_PREFIX} User with phone or username {x} was not found!";
        public static readonly string InsufficientFundsError = $"{ERROR_PREFIX} Insufficient funds";
        public static readonly string AutoSendCreditsError = $"{ERROR_PREFIX} Not allowed to send credits to yourself!";
        public static readonly string StatisticsCasheName = "StatisticsStore";


        public static Func<DateTime, string> FormatDateTime = (x) => x.ToString("dd/MM/yyyy hh:mm");

    }
}
