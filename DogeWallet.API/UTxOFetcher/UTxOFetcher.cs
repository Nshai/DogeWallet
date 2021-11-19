using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DogeWallet.API.UTxOFetcher
{
    public abstract class UTxOFetcher
    {
        protected HttpClient HttpClient { get; set; }
        public UTxOFetcher()
        {
            this.HttpClient = new HttpClient();
        }

    }
}
