using CommonLibrary.Interfaces;
using Infrasturcture.Models;
using System;

namespace Infrastructure.DTOS
{
    public class TransferInfoDTOout : IMapFrom<CreditTransfer>
    {
        public string SenderUserName { get; set; }
      //  public string SenderPhoneNumber { get; set; }
        public string RecieverUserName { get; set; }
      //  public string RecieverPhoneNumber { get; set; }
        public string Comment { get; set; }
        public decimal Ammount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
