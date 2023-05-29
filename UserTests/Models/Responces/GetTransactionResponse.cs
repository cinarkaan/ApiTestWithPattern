using WalletTests.Enum;

namespace WalletTests.Models.Responces
{
    public class GetTransactionsResponse
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionId { get; set; }

        public string Time { get; set; }

        public UserStatus Status { get; set; }

        public string BaseTransactionId { get; set; }

    }
}
