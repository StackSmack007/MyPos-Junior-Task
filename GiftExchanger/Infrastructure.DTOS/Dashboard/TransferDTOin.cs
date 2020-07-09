using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOS
{
    public class TransferDTOin
    {
        [Required]
        public string RecieverPhoneOrUsername { get; set; }
        public string Comment { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Ammount { get; set; }
    }
}