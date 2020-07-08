using AutoMapper.Configuration.Annotations;
using CommonLibrary.Interfaces;
using Infrasturcture.Models;

namespace Infrastructure.DTOS
{
    public class UserDataDTOout : IMapFrom<UserGE>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal CreditBalance { get; set; }
        public int TransactionsSentCount { get; set; }
        public int TransactionsRecievedCount { get; set; }
        //[Ignore]
        //public bool IsAdmin { get; set; } = false;
    }
}
