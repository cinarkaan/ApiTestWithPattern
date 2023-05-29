using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTests.Models.Requests
{
    public class RegisterUserRequest
    {

        [JsonProperty("firstName")]
        public string Name;
        [JsonProperty("lastName")]
        public string Surname;


    }
}
