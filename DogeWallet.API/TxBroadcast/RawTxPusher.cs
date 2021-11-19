
using System.Net.Http;
using System.Threading.Tasks;

namespace DogeWallet.API.TxBroadcast
{
    public abstract class RawTxPusher
    {
        protected HttpClient HttpClient { get; }

        public RawTxPusher()
        {
            HttpClient = new HttpClient();
        }

        public abstract Task<string> BroadcastRawTx(string broadcastUrl, string rawTx);
    }
}
