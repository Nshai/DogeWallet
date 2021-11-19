using DogeWallet.API.TxBroadcast.Doge;
using DogeWallet.API.UTxOFetcher.Doge;
using DogeWallet.Wallet.Transactions;
using NBitcoin;
using NBitcoin.Altcoins;
using System;
using System.Threading.Tasks;

namespace DogeWallet.Wallet.Wallets.Doge
{
    public class DogeMainnetWallet : Wallet
    {
        public DogeMainnetWallet(string passAndSalt, ScriptPubKeyType addrType) :
                               base(passAndSalt, addrType)
        {
            this.Network = Dogecoin.Instance.Testnet;
        }


        public override async Task<string> PushRawTxAsync(string toAddr, decimal amount, decimal fee, bool isDonationEnabled,
                                                          string donationAddress, decimal donationAmount)
        {
            try
            {
                var errorMsg = TxInputValidation(amount, fee, toAddr);
                if (errorMsg != null) return errorMsg;

                var dogeUTxOFetcher = new DogeUTxOFetcher();
                var allUtxos = await getUTxOs(dogeUTxOFetcher, DogeURLS.DogeMainnetUTxO);

                var txModel = FillTxModel(allUtxos, toAddr, donationAddress, amount, fee,
                                          donationAmount, isDonationEnabled);
                if (!haveEnoughFunds(txModel)) return "Insufficient Funds";

                var rawTx = new RawTx();
                var txPusher = new DogeTxPusher();
                var txSignature = rawTx.SignRawTx(txModel);

                var txResult = await txPusher.BroadcastRawTx(DogeURLS.DogeMainnetBroadcast, txSignature);
                return txResult;
            }
            catch (Exception)
            {
                return "Something Went Wrong.";
            }
        }
    }
}
