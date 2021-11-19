using System.Text;
using System.Security.Cryptography;
namespace DogeWallet.Wallet.Sha256
{
    internal static class SHA256
    {
        internal static byte[] GenerateHashBytes(this string passAndSalt)
        {
            // Convert the string to a byte array
            byte[] textBytes = Encoding.UTF8.GetBytes(passAndSalt);
            byte[] hashBytes = new SHA256Managed().ComputeHash(textBytes);
           
            //returns 32bytes == 256 bits
            return hashBytes;
        }
    }
}
