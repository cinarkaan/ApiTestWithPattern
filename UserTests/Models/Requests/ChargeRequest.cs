using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletTests.Models.Requests
{
    public class ChargeRequest
    {

        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; } 

    }
}
