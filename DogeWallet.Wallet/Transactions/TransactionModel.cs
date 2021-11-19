using DogeWallet.API.UTxOFetcher;
using NBitcoin;
using NBitcoin.Altcoins;


namespace DogeWallet.Wallet.Transactions
{
    public class TransactionModel
    {
        public IUTxO[] utxos { get; set; }
        public string ReceivedAddress { get; set; }
        public string DonationAddress { get; set; }
        public decimal DonationsAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public Network Network { get; set; }
        public Key Key { get; set; }
        public bool IsDonationsEnabled { get; set; } 
        public ScriptPubKeyType AddressType { get; set; }

        public decimal GetTotalAmount()
        {
            return Amount + DonationsAmount + Fee;
        }
    }
}
