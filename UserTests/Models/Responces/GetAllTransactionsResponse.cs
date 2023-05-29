using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletTests.Models.Responces;

namespace UserTests.Models.Responces
{
    public class GetAllTransactionsResponse
    {
        [JsonProperty("")]
        public GetTransactionsResponse TransactionsResponse { get; set; }

    }
}
