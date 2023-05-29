using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTests.Extensions;
using UserTests.Models.Requests;
using UserTests.Models.Responces;
using UserTests.Models.Responces.Base;

namespace UserTests.Clients
{
    public class UserServiceClient : BaseClientService , IObservable<int>
    {

        private readonly ConcurrentBag<IObserver<int>> _observers = new ConcurrentBag<IObserver<int>>();
        private static readonly Lazy<UserServiceClient> _lazy = new Lazy<UserServiceClient>(() => new UserServiceClient());

        public static UserServiceClient Instance = _lazy.Value;

        public UserServiceClient ()
        {
            _baseUrl = "https://userservice-uat.azurewebsites.net";
        }

        public async Task<CommonResponse<object>> RegisterUser(RegisterUserRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/Register/RegisterNewUser"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
                NotifyAllObservers(int.Parse(response.Content.ReadAsStringAsync().Result));

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<object>> GetUserStatus(int id)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/GetUserStatus?userId={id}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<object>> DeleteUser(int id)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUrl}/Register/DeleteUser?userId={id}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<object>> UpdateUser(int id,bool status)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/SetUserStatus?userId={id}&newStatus={status}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<GetAllUsersResponse>> GetAllUsers (GetAllUsersRequests request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/GetAllUsers"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };


            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<GetAllUsersResponse>();
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _observers.Add(observer);
            return null;
        }

        public void NotifyAllObservers (int id)
        {
            foreach(IObserver<int> observer in _observers)
            {
                observer.OnNext(id);
            }
        }
    }
}
