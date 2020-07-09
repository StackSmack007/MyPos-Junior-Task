using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOS
{
    public class TransferDTOin
    {
        [Required, StringLength(16, MinimumLength = 4)]
        public string RecieverUnameOrPhone { get; set; }
        public string Comment { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335",ErrorMessage ="The ammount must be larger than 0.01")]
        public decimal Ammount { get; set; }
    }
}