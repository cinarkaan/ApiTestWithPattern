using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTests.Clients
{
    public class BaseClientService
    {

        protected readonly HttpClient _httpClient = new HttpClient(); 
        protected string _baseUrl = "";
        
    }
}
