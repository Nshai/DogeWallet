using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeWallet.Wallet.Wallets.Doge
{
    internal static class DogeURLS
    {
        // Testnet
        internal static string DogeTestnetBroadcast { get; } = "https://chain.so/api/v2/send_tx/DOGETEST/";
        internal static string DogeTestnetUTxO { get; } = "https://chain.so/api/v2/get_tx_unspent/DOGETEST/";

        // Mainnet
        internal static string DogeMainnetBroadcast { get; } = "https://chain.so/api/v2/send_tx/DOGE/";
        internal static string DogeMainnetUTxO { get; } = "https://chain.so/api/v2/get_tx_unspent/DOGE/";

    }
}
