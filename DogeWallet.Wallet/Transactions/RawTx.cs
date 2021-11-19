using DogeWallet.API.UTxOFetcher;
using NBitcoin;
using NBitcoin.Altcoins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeWallet.Wallet.Transactions
{
    internal class RawTx
    {
        internal string SignRawTx(TransactionModel txModel)
        {
            var sendToAddr = BitcoinAddress.Create(txModel.ReceivedAddress, txModel.Network);
            var donationAddress = BitcoinAddress.Create(txModel.DonationAddress, txModel.Network);

            var utxo = pickBestUtxos(txModel.utxos, txModel.GetTotalAmount());

            var transaction = Transaction.Create(txModel.Network);
            generateOutpoints(transaction, utxo);
            generateMoney(transaction, utxo, txModel, sendToAddr, donationAddress,txModel.AddressType);

            signTrans(transaction, utxo, txModel.Key, txModel.Network, txModel.AddressType);

            return transaction.ToHex();
        }

        //     Finding the best UTxOs for incomming transaction
        private IUTxO[] pickBestUtxos(IUTxO[] utxos, decimal totalValue)
        {
            List<IUTxO> bestUtxos = new();
            decimal currValue = 0m;
            foreach (var utxo in utxos)
            {
                if (currValue >= totalValue) break;
                currValue += Convert.ToDecimal(utxo.GetValue());
                bestUtxos.Add(utxo);
            }
            return bestUtxos.ToArray();
        }

        //add outpoints in transaction
        private void generateOutpoints(Transaction tr, IUTxO[] utxos)
        {
            foreach (var utxo in utxos)
            {
                OutPoint otp = OutPoint.Parse(utxo.GetTxId() + ":" + utxo.GetOutputNo());
                tr.Inputs.Add(otp);
            }
        }

        //add coins to transaction
        private void generateMoney(Transaction tx, IUTxO[] utxos, TransactionModel txModel,
                                   BitcoinAddress sendToAddr, BitcoinAddress donationAddress, ScriptPubKeyType addrType)
        {
            if (txModel.IsDonationsEnabled)
            {
                generateMoneyWithDonation(tx, utxos, txModel, sendToAddr,
                                         donationAddress,addrType);
                return;
            }

            generateMoneyWithoutDonations(tx, utxos, txModel, sendToAddr,txModel.AddressType);

        }

        private void generateMoneyWithDonation(Transaction tx, IUTxO[] utxos, TransactionModel txModel,
                                                BitcoinAddress sendToAddr, BitcoinAddress donationAddress,
                                                ScriptPubKeyType addrType)
        {
            decimal rest = utxosTotalBalance(utxos) - (txModel.Amount + txModel.Fee + txModel.DonationsAmount);
            var forAddr = new Money(txModel.Amount, MoneyUnit.BTC);
            var forDonation = new Money(txModel.DonationsAmount, MoneyUnit.BTC);

            if (rest > 0)
            {
                Money restMoney = new(rest, MoneyUnit.BTC);
                tx.Outputs.Add(forAddr, sendToAddr.ScriptPubKey);
                tx.Outputs.Add(forDonation, donationAddress.ScriptPubKey);
                tx.Outputs.Add(restMoney, txModel.Key.PubKey.GetAddress(addrType, txModel.Network).ScriptPubKey);
                for (int i = 0; i < utxos.Count(); i++)
                {
                    tx.Inputs[i].ScriptSig = txModel.Key.PubKey.GetAddress(addrType, txModel.Network).ScriptPubKey;
                }
                return;
            }
            tx.Outputs.Add(forAddr, sendToAddr.ScriptPubKey);
            tx.Outputs.Add(forDonation, donationAddress.ScriptPubKey);

            //sign all inputs
            for (int i = 0; i < utxos.Count(); i++)
            {
                tx.Inputs[i].ScriptSig = txModel.Key.PubKey.GetAddress(addrType, txModel.Network).ScriptPubKey;
            }
        }


        private void generateMoneyWithoutDonations(Transaction tx, IUTxO[] utxos, TransactionModel txModel,
                                                BitcoinAddress sendToAddr, ScriptPubKeyType addrType)
        {
            decimal rest = utxosTotalBalance(utxos) - (txModel.Amount + txModel.Fee);
            var forAddr = new Money(txModel.Amount, MoneyUnit.BTC);

            if (rest > 0)
            {
                Money restMoney = new(rest, MoneyUnit.BTC);
                tx.Outputs.Add(forAddr, sendToAddr.ScriptPubKey);
                tx.Outputs.Add(restMoney, txModel.Key.PubKey.GetAddress(addrType, txModel.Network).ScriptPubKey);
                for (int i = 0; i < utxos.Count(); i++)
                {
                    tx.Inputs[i].ScriptSig = txModel.Key.PubKey.GetAddress(addrType, txModel.Network).ScriptPubKey;
                }
                return;
            }
            tx.Outputs.Add(forAddr, sendToAddr.ScriptPubKey);

            //sign all inputs
            for (int i = 0; i < utxos.Count(); i++)
            {
                tx.Inputs[i].ScriptSig = txModel.Key.PubKey.GetAddress(addrType, txModel.Network).ScriptPubKey;
            }
        }

        //get total balance from all inputs in use for next transaction
        private decimal utxosTotalBalance(IUTxO[] utxos)
        {
            decimal res = 0;
            foreach (var utxo in utxos)
            {
                res += Convert.ToDecimal(utxo.GetValue());
            }
            return res;
        }

        private void signTrans(Transaction tr, IUTxO[] utxos, Key key, Network network, ScriptPubKeyType addrType)
        {
            List<ICoin> coins = new();
            foreach (var utxo in utxos)
            {
                var txInString = uint256.Parse(utxo.GetTxId());
                var coin = new Coin(txInString, utxo.GetOutputNo(),
                                        new Money(Convert.ToDecimal(utxo.GetValue()), MoneyUnit.BTC),
                                        key.PubKey.GetAddress(addrType, network).ScriptPubKey);

                coins.Add(coin);
            }
            tr.Sign(key.GetWif(network), coins);
        }

    }
}
