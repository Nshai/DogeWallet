using DogeWallet.API.UTxOFetcher.UnspentTxOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeWallet.API.UTxOFetcher.Doge
{
    public class DogeUTxOFetcher : UTxOFetcher , IUTxOFetcher
    {
        public DogeUTxOFetcher() : base()
        {
        }
        public  async Task<IUTxO[]> FetchUTxOAsync(string UTxOURL,string address)
        {
            var apiCall = await HttpClient.GetAsync(UTxOURL + address);
            var asStr = await apiCall.Content.ReadAsStringAsync();
            var UTxOs = System.Text.Json.JsonSerializer.Deserialize<Data>(asStr);

            return UTxOs.data.txs;
        }
    }
}
