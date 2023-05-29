using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTests.Models.Requests;
using WalletTests.Models.Requests;

namespace UserTests.Utils
{
    public class WalletCharger
    {

        public ChargeRequest ChargeWallet(int UserID, decimal amount)
        {
            return new ChargeRequest()
            {
                UserId = UserID,
                Amount = amount,
            };
        }

    }
}
