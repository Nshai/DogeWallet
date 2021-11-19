using DogeWallet.API.TxBroadcast.TransactionResult;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace DogeWallet.API.TxBroadcast.Doge
{
    public class DogeTxPusher : RawTxPusher
    {
        public DogeTxPusher() : base()
        {
        }
        public override async Task<string> BroadcastRawTx(string broadcastUrl, string rawTx)
        {
            try
            {
                if (string.IsNullOrEmpty(rawTx)) return null;

                var client = new RestClient(broadcastUrl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddJsonBody(new { tx_hex = rawTx });
                var response = await client.ExecuteAsync(request);
                var transHash = System.Text.Json.JsonSerializer.Deserialize<TxData>(response.Content);
                if (string.IsNullOrEmpty(transHash.data.txid)) return null;

                return transHash.data.txid;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
