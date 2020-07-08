namespace Infrasturcture.Models
{
    using CommonLibrary;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    public class UserGE : IdentityUser
    {
        public UserGE()
        {
            this.CreditBalance = GlobalConstants.InitialRegisterCredit;
            TransactionsSent = new HashSet<CreditTransfer>();
            TransactionsRecieved = new HashSet<CreditTransfer>();
        }

        public decimal CreditBalance { get; set; }
        public virtual ICollection<CreditTransfer> TransactionsSent { get; set; }
        public virtual ICollection<CreditTransfer> TransactionsRecieved { get; set; }
    }
}