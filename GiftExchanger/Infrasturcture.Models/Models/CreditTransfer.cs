using Infrastructure.Models;

namespace Infrasturcture.Models
{
    public class CreditTransfer : BaseStatsId<int>
    {
        public string SenderId { get; set; }
        public virtual UserGE Sender { get; set; }

        public string RecieverId { get; set; }
        public virtual UserGE Reciever { get; set; }

        //[MaxLength(1024)]
        public string Comment { get; set; }
    }
}
