

using Newtonsoft.Json;

namespace UserTests.Models.Responces
{
    public class GetAllUsersResponse
    {
        public int PageNumber { get; set; }

        public int PageTotal { get; set; }

        public GetUserResponse[] result { get; set; }


    }
}
