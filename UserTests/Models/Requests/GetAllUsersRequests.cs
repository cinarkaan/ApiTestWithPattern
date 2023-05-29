using Newtonsoft.Json;


namespace UserTests.Models.Requests
{
    public class GetAllUsersRequests
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("filter")]
        public RegisterUserRequest Filter { get; set; }


    }
}
