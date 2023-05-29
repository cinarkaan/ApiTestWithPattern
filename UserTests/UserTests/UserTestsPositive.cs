using NUnit.Framework;
using System.Net;
using UserTests.Clients;
using UserTests.Utils;

namespace UserTests.UserTests
{

    [TestFixture]
    public class UserTestsPositive
    {

        private readonly UserServiceClient _userServiceClient = UserServiceClient.Instance;
        private readonly UserGenerator _userGenerator = new UserGenerator();

        [Test]
        public async Task CreateValidProduct_CreateWithEmptyFields_StatusCodeIsOk()
        {

            var request = _userGenerator.GenerateRegisterUserRequest("", "");

            var responce = await _userServiceClient.RegisterUser(request);

            Console.WriteLine(responce.Body);

            Assert.AreEqual(HttpStatusCode.OK, responce.Status);
        }

        [Test]
        public async Task CreateValidProduct_CreateWithDigitFields_StatusCodeIsOk()
        {

            var name = _userGenerator.GetRandomDigits(5);

            var surname = _userGenerator.GetRandomDigits(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responce = await _userServiceClient.RegisterUser(request);

            Assert.AreEqual(HttpStatusCode.OK, responce.Status);
        }

        [Test]
        public async Task CreateValidProduct_CreateWithSpecialCharacters_StatusCodeIsOk()
        {
            var name = _userGenerator.GetRandomSpecialChars(5);

            var surname = _userGenerator.GetRandomSpecialChars(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responce = await _userServiceClient.RegisterUser(request);

            Assert.AreEqual(HttpStatusCode.OK, responce.Status);
        }

        [Test]
        public async Task CreateValidProduct_CreateWithLenghtOneSymbol_StatusCodeIsOk()
        {

            var request = _userGenerator.GenerateRegisterUserRequest("K", "C");

            var responce = await _userServiceClient.RegisterUser(request);

            Assert.AreEqual(HttpStatusCode.OK, responce.Status);
        }

        [Test]
        public async Task CreateValidProduct_CreateWithLenghtOneHundredSymbol_StatusCodeIsOk()
        {

            string name = _userGenerator.GetRandomString(120);
            string surname = _userGenerator.GetRandomString(120);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responce = await _userServiceClient.RegisterUser(request);

            Assert.AreEqual(HttpStatusCode.OK, responce.Status);
        }


        [Test]
        public async Task CreateValidProduct_CreateWithUpperCaseCharacter_StatusCodeIsOk()
        {
            var request = _userGenerator.GenerateRegisterUserRequest("Kaan".ToUpper(), "Cinar".ToUpper());

            var responce = await _userServiceClient.RegisterUser(request);

            Assert.AreEqual(HttpStatusCode.OK, responce.Status);
        }

        [Test]
        public async Task CreateValidProduct_ReturningValueIsAutoIncremented_StatusCodeIsOk()
        {
            var request1 = _userGenerator.GenerateRegisterUserRequest("Serhii", "Mykhailov");

            var responce1 = await _userServiceClient.RegisterUser(request1);

            int autoIncValue1 = int.Parse(responce1.Content);

            var request2 = _userGenerator.GenerateRegisterUserRequest("Kaan", "Cinar");

            var responce2 = await _userServiceClient.RegisterUser(request2);

            int autoIncValue2 = int.Parse(responce2.Content);

            Assert.AreEqual(1, autoIncValue2 - autoIncValue1);
        }


        [Test]
        public async Task GetUserStatus_IsDefaultStatusFalse_StatusCodeIsOk()
        {
            var request = _userGenerator.GenerateRegisterUserRequest("Cinar", "Kaan");

            var responceCreateUser = await _userServiceClient.RegisterUser(request);

            int userID = int.Parse(responceCreateUser.Content);

            var responce = await _userServiceClient.GetUserStatus(userID);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responce.Status);
                Assert.AreEqual("false", responce.Content);
            });

        }


        [Test]
        public async Task GetUserStatus_CheckChangedFalseStatus_StatusCodeIsOk()
        {
            int userId = 166036;

            var responceGetUser = await _userServiceClient.GetUserStatus(userId);

            string before = responceGetUser.Content;

            var responceUpdateStatus = await _userServiceClient.UpdateUser(userId, false);

            responceGetUser = await _userServiceClient.GetUserStatus(userId);

            string after = responceGetUser.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("true", before);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateStatus.Status);
                Assert.AreEqual("false", after);
            });

        }

        [Test]
        public async Task GetUserStatus_CheckChangedTrueStatus_StatusCodeIsOk()
        {

            int userId = 166036;

            var responceGetUser = await _userServiceClient.GetUserStatus(userId);

            string before = responceGetUser.Content;

            var responceUpdateStatus = await _userServiceClient.UpdateUser(userId, true);

            responceGetUser = await _userServiceClient.GetUserStatus(userId);

            string after = responceGetUser.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("false", before);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateStatus.Status);
                Assert.AreEqual("true", after);
            });

        }



        [Test]
        public async Task DeleteUser_NotActiveUser_StatusCodeIsOk()
        {
            var name = _userGenerator.GetRandomString(5);

            var surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceCreateUser = await _userServiceClient.RegisterUser(request);

            int userID = int.Parse(responceCreateUser.Content);

            var responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var responceDeleteUser = await _userServiceClient.DeleteUser(userID);


            Assert.Multiple(() =>
            {
                Assert.AreEqual("false", responceGetUserID.Content);
                Assert.AreEqual(HttpStatusCode.OK, responceDeleteUser.Status);
            });
        }


        [Test]
        public async Task SetUserStatus_FalseToTrue_StatusCodeIsOk()
        {

            var name = _userGenerator.GetRandomString(5);

            var surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceCreateUser = await _userServiceClient.RegisterUser(request);

            int userID = int.Parse(responceCreateUser.Content);

            var responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var statusBefore = responceGetUserID.Content;

            var responceUpdateUser = await _userServiceClient.UpdateUser(userID, true);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var statusNext = responceGetUserID.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("false", statusBefore);
                Assert.AreEqual(HttpStatusCode.OK, responceCreateUser.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("true", statusNext);
            });
        }

        [Test]
        public async Task SetUserStatus_FalseToFalse_StatusCodeIsOk()
        {

            var name = _userGenerator.GetRandomString(5);

            var surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceCreateUser = await _userServiceClient.RegisterUser(request);

            int userID = int.Parse(responceCreateUser.Content);

            var responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var statusBefore = responceGetUserID.Content;

            var responceUpdateUser = await _userServiceClient.UpdateUser(userID, false);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var statusNext = responceGetUserID.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("false", statusBefore);
                Assert.AreEqual(HttpStatusCode.OK, responceCreateUser.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("false", statusNext);
            });
        }

        [Test]
        public async Task SetUserStatus_FalseToTrueToFalse_StatusCodeIsOk()
        {

            var name = _userGenerator.GetRandomString(5);

            var surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceCreateUser = await _userServiceClient.RegisterUser(request);

            int userID = int.Parse(responceCreateUser.Content);

            var responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var status1 = responceGetUserID.Content;

            var responceUpdateUser = await _userServiceClient.UpdateUser(userID, true);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var status2 = responceGetUserID.Content;

            responceUpdateUser = await _userServiceClient.UpdateUser(userID, false);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var status3 = responceGetUserID.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("false", status1);
                Assert.AreEqual(HttpStatusCode.OK, responceCreateUser.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("true", status2);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("false", status3);
            });
        }

        [Test]
        public async Task SetUserStatus_FalseToTrueToFalseToTrue_StatusCodeIsOk()
        {

            var name = _userGenerator.GetRandomString(5);

            var surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceCreateUser = await _userServiceClient.RegisterUser(request);

            int userID = int.Parse(responceCreateUser.Content);

            var responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var status1 = responceGetUserID.Content;

            var responceUpdateUser = await _userServiceClient.UpdateUser(userID, true);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var status2 = responceGetUserID.Content;

            responceUpdateUser = await _userServiceClient.UpdateUser(userID, false);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var status3 = responceGetUserID.Content;

            responceUpdateUser = await _userServiceClient.UpdateUser(userID, true);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var status4 = responceGetUserID.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("false", status1);
                Assert.AreEqual(HttpStatusCode.OK, responceCreateUser.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("true", status2);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("false", status3);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("true", status4);
            });
        }

        [Test]
        public async Task SetUserStatus_TrueToTrue_StatusCodeIsOk()
        {

            int userID = 166071;

            var responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var statusPrevious = responceGetUserID.Content;

            var responceUpdateUser = await _userServiceClient.UpdateUser(userID, true);

            responceGetUserID = await _userServiceClient.GetUserStatus(userID);

            var statusNext = responceGetUserID.Content;

            Assert.Multiple(() =>
            {
                Assert.AreEqual("true", statusPrevious);
                Assert.AreEqual(HttpStatusCode.OK, responceUpdateUser.Status);
                Assert.AreEqual("true", statusNext);
            });
        }


    }
}
