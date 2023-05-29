using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserTests.Clients;
using UserTests.Extensions;
using UserTests.Models.Responces.Base;
using WalletTests.Models.Requests;
using WalletTests.Models.Responces;

namespace WalletTests.Clients
{
    public class WalletServiceClient : BaseClientService
    {

        public WalletServiceClient ()
        {
            _baseUrl = "https://walletservice-uat.azurewebsites.net";
        }

        public async Task<CommonResponse<object>> GetBalance(int id)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/Balance/GetBalance?userId={id}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<GetTransactionsResponse>> GetTransactions(int id)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/Balance/GetTransactions?userId={id}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponseArr<GetTransactionsResponse>();
        }

        public async Task<CommonResponse<object>> Charge(ChargeRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/Balance/Charge"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<object>> RevertTransaction(string transactionId)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUrl}/Balance/RevertTransaction?transactionId={transactionId}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponse<object>();
        }
    }
}
