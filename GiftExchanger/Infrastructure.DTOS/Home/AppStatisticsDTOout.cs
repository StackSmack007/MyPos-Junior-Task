using System.Collections.Generic;

namespace Infrastructure.DTOS
{
    public class AppStatisticsDTOout
    {
        public virtual ICollection<string> AllUserNames { get; set; } = new HashSet<string>();
        public virtual ICollection<string> AdminUserNames { get; set; } = new HashSet<string>();
        public int TotalTransactions { get; set; }
        public decimal TotalTransferedCredits { get; set; }
    }
}