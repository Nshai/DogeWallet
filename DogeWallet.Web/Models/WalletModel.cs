using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogeWallet.Web.Models
{
    public class WalletModel
    {
        public string Address { get; set; }
        public TransactionModel TransactionModel { get; set; }
    }
}
