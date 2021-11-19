using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeWallet.API.UTxOFetcher
{
    public interface IUTxOFetcher
    {
        Task<IUTxO[]> FetchUTxOAsync(string UTxOURL, string address);
    }
}
