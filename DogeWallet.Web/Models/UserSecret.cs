using System.ComponentModel.DataAnnotations;

namespace DogeWallet.Web.Models
{
    public class UserSecret
    {
        [Required]
        [MinLength(10)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Pin must contains 6 digits.")]
        [MinLength(6, ErrorMessage = "Pin must contains 6 digits.")]
        [Range(0, 999999,ErrorMessage ="Pin must contains 6 digits.")]
        public string Pin { get; set; }

    }
}