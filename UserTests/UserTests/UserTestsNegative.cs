using NUnit.Framework;
using System.Net;
using UserTests.Clients;
using UserTests.Utils;

namespace UserTests.UserTests
{
    [TestFixture]
    public class UserTestsNegative
    {

        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();

        [Test]
        public async Task SetUserStatus_NonExistUser_StatusCodeIsInternalServerError()
        {
            var responce = await _userServiceClient.UpdateUser(000000, true);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responce.Status);
                Assert.AreEqual("Sequence contains no elements", responce.Content);
            });
        }

        [Test]
        public async Task CreateInValidProduct_CreateWithNullFields_StatusCodeIsInternalServerError()
        {
            var request = _userGenerator.GenerateRegisterUserRequest(null, null);

            var responce = await _userServiceClient.RegisterUser(request);

            Assert.AreEqual(HttpStatusCode.InternalServerError, responce.Status);
        }

        [Test]
        public async Task GetUserStatus_NonExistsUser_StatusCodeIsInternalServerError()
        {

            var responce = await _userServiceClient.GetUserStatus(000000);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responce.Status);
                Assert.AreEqual("Sequence contains no elements", responce.Content);
            });

        }

        [Test]
        public async Task DeleteUser_NonExistUser_StatusCodeIsInternalServerError()
        {
            var responce = await _userServiceClient.DeleteUser(000000);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responce.Status);
                Assert.AreEqual("Sequence contains no elements", responce.Content);
            });
        }

    }
}
