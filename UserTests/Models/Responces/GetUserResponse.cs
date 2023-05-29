
using WalletTests.Enum;

namespace UserTests.Models.Responces
{
    public class GetUserResponse
    {

        public Int32 Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserStatus IsActive { get; set; } 

    }
}
