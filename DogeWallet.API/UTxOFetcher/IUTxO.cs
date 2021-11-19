using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeWallet.API.UTxOFetcher
{
    public interface IUTxO
    {
        string GetTxId();
        uint GetOutputNo();
        string GetValue();
    }
}
