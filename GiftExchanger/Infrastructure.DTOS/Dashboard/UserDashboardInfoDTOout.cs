using System.Collections.Generic;

namespace Infrastructure.DTOS
{
    public class UserDashboardInfoDTOout
    {
        public string UserName { get; set; }
        public decimal CreditBalance { get; set; }
        public virtual ICollection<TransferDTOout> Sent { get; set; } = new HashSet<TransferDTOout>();
        public virtual ICollection<TransferDTOout> Recieved { get; set; } = new HashSet<TransferDTOout>();
    }
}