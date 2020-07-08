using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOS
{
    public class CreditAdditionDTOin
    {
        public string RecieverId { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Ammount { get; set; }
    }
}