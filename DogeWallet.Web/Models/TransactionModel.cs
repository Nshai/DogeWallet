using System.ComponentModel.DataAnnotations;

namespace DogeWallet.Web.Models
{
    public class TransactionModel
    {
        [Required]
        public string ToAddress { get; set; }
        [Required]
        [Range(typeof(decimal), "0.00000001", "79228162514264337593543950335")]
        public decimal Amount { get; set; }
        [Required]
        [Range(typeof(decimal), "0.00000001", "79228162514264337593543950335")]
        public decimal Fee { get; set; }
        public bool IncludeDonation { get; set; }

        public string TransactionResult { get; set; } = "Something Went Wrong.";
    }
}