namespace Infrastructure.DTOS
{
    public class AppStatisticsDTOout
    {
        public int TotalUsersCount { get;  set; }
        public int AdminsUsersCount { get;  set; }
        public int TotalTransactions { get;  set; }
        public decimal TotalTransferedCredits { get;  set; }
    }
}