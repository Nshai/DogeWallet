using DogeWallet.API.UTxOFetcher;
using DogeWallet.Wallet.Sha256;
using DogeWallet.Wallet.Transactions;
using NBitcoin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DogeWallet.Wallet.Wallets
{
    public abstract class Wallet
    {
        protected Network Network { get; set; }
        protected Key OwnerKey { get; }
        public BitcoinAddress OwnerAddress { get; }
        public ScriptPubKeyType AddressType { get; set; }

        public Wallet(string passAndSalt, ScriptPubKeyType addrType)
        {
            this.Network = Network;
            OwnerKey = new Key(passAndSalt.GenerateHashBytes());
            AddressType = addrType;
        }

        /// <summary>
        /// Get Readable Private Key.
        /// </summary>
        /// <returns>Private key in string format.</returns>
        public string GetWIF()
        {
            return OwnerKey.GetWif(Network).ToString();
        }

        /// <summary>
        /// Get Readable Address.
        /// </summary>
        /// <returns>Address in string format.</returns>
        public string GetAddress()
        {
            return OwnerKey.GetAddress(AddressType, Network).ToString();
        }



        /// <summary>
        ///  Create, sign and push Tx to the network.
        /// </summary>
        /// <param name="toAddr"></param>
        /// <param name="amount"></param>
        /// <param name="fee"></param>
        /// <param name="isDonationEnabled"></param>
        /// <param name="donationAddress"></param>
        /// <param name="donationAmount"></param>
        /// <returns>Hexadecimal transaction result.</returns>
        public abstract Task<string> PushRawTxAsync(string toAddr, decimal amount, decimal fee,
                                                bool isDonationEnabled, string donationAddress,
                                                decimal donationAmount);

#nullable enable
        // Check if Tx is valid. 
        // if Tx is invalid return error message
        protected string? TxInputValidation(decimal amount, decimal fee, string toAddr)
        {
            if (amount < 0.00000001m) return "Amount Must Be Greater Than 0.00000001";
            if (fee < 0.00000001m) return "Fee Must Be Greater Than 0.00000001";
            if (!isAddressValid(toAddr, Network)) return "Invali Address";
            return null;
        }
#nullable disable


        protected TransactionModel FillTxModel(IUTxO[] allUtxos, string toAddr, string donationAddress,
                                               decimal amount, decimal fee, decimal donationAmount,
                                               bool isDonationEnabled)
        {
            return new TransactionModel()
            {
                utxos = allUtxos,
                ReceivedAddress = toAddr,
                DonationAddress = donationAddress,
                DonationsAmount = donationAmount,
                Amount = amount,
                Fee = fee,
                Network = Network,
                Key = OwnerKey,
                IsDonationsEnabled = isDonationEnabled
            };
        }

        // fetching unspent transactions outputs
        protected async Task<IUTxO[]> getUTxOs(IUTxOFetcher utxoFetcher, string UTxOURL)
        {
            var utxos = await utxoFetcher.FetchUTxOAsync(UTxOURL, GetAddress());
            return utxos.OrderBy(utxo => Convert.ToDecimal(utxo.GetValue())).Reverse().ToArray();
        }

        protected bool haveEnoughFunds(TransactionModel txModel)
        {
            decimal fundsFromUtxos = 0m;
            decimal toSendWithDonations = txModel.Amount + txModel.DonationsAmount + txModel.Fee;
            decimal toSendWithoutDonations = txModel.Amount + txModel.Fee;
            foreach (var utxo in txModel.utxos)
            {
                fundsFromUtxos += Convert.ToDecimal(utxo.GetValue());
            }
            if (txModel.IsDonationsEnabled) return fundsFromUtxos >= toSendWithDonations;
            return fundsFromUtxos >= toSendWithoutDonations;
        }

        protected bool isAddressValid(string addr, Network network)
        {
            try
            {
                BitcoinAddress address = BitcoinAddress.Create(addr, network);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


    }
}
