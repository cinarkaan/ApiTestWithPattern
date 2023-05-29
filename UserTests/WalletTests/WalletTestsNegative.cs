using NUnit.Framework;
using System.Net;
using UserTests.Clients;
using UserTests.Utils;
using WalletTests.Clients;

namespace UserTests.WalletTests
{
    [TestFixture]
    public class WalletTestsNegative
    {
        private readonly WalletServiceClient _walletServicesClient = new WalletServiceClient();
        private readonly WalletCharger _walletCharger = new WalletCharger();
        private readonly UserServiceClient _userServiceClient = UserServiceClient.Instance;
        private readonly UserGenerator _userGenerator = new UserGenerator();


        [Test]
        public async Task GetBalance_NewUserBalance_StatusCodeIsInternalServerError()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            var responceGetBalance = await _walletServicesClient.GetBalance(int.Parse(responceRegister.Content));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responceGetBalance.Status);
                Assert.AreEqual("not active user", responceGetBalance.Content);
            });
        }

        [Test]
        public async Task GetBalance_NoActiveUser_StatusCodeIsInternalServerError()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            var responseStatus = await _userServiceClient.GetUserStatus(int.Parse(responceRegister.Content));

            var responceGetBalance = await _walletServicesClient.GetBalance(int.Parse(responceRegister.Content));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, responceGetBalance.Status);
                Assert.AreEqual(false, responseStatus.Body);
                Assert.AreEqual("not active user", responceGetBalance.Content);
            });
        }

        [Test]
        public async Task GetBalance_OneTransaction_OveralBalance_MinusTenKKDotZeroOne_StatusCodeInternalServerError()
        {
            decimal amount = -10000000.01m;

            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            int UserId = int.Parse(responceRegister.Content);

            var responceSetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestCharge = _walletCharger.ChargeWallet(UserId, amount);

            var responceGetBalance = await _walletServicesClient.Charge(requestCharge);

            var responceGetTransactions = await _walletServicesClient.GetTransactions(UserId);

            var balanceRequest = await _walletServicesClient.GetBalance(UserId);


            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceRegister.Status);
                Assert.AreEqual(HttpStatusCode.InternalServerError, responceGetBalance.Status);
            });
        }



        [Test]
        public async Task Charge_NotActiveUser_StatusCodeIsInternalServerError()
        {

            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var requestRegister = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(requestRegister);

            int UserId = int.Parse(responceRegister.Content);

            var responceGetStatus = await _userServiceClient.GetUserStatus(UserId);

            var requestCharge = _walletCharger.ChargeWallet(UserId, 25000);

            var responceGetBalance = await _walletServicesClient.Charge(requestCharge);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("false", responceGetStatus.Content);
                Assert.AreEqual(HttpStatusCode.InternalServerError, responceGetBalance.Status);
                Assert.AreEqual("not active user", responceGetBalance.Content);
            });

        }

        [Test]
        public async Task Charge_BalanceZeroChargeMinusThirty_StatusCodeIsInternalServerError()
        {
            decimal Amount = -30m;

            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var requestRegister = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(requestRegister);

            int UserId = int.Parse(responceRegister.Content);

            var responceGetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var balance = await _walletServicesClient.GetBalance(UserId);

            var requestCharge = _walletCharger.ChargeWallet(UserId, Amount);

            var responceCharge = await _walletServicesClient.Charge(requestCharge);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(0, balance.Body);
                Assert.AreEqual(HttpStatusCode.InternalServerError, responceCharge.Status);
                Assert.AreEqual($"User have '0', you try to charge '{Amount + ".0"}'.", responceCharge.Content);
            });
        }

        [Test]
        public async Task Charge_BalanceNChargeMinusNMinusZeroDotZeroOne_StatusCodeIsInternalServerError()
        {
            decimal OriginBalance = 1500m;
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var requestRegister = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(requestRegister);

            int UserId = int.Parse(responceRegister.Content);

            var responceGetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestChargeN = _walletCharger.ChargeWallet(UserId, OriginBalance);

            var responceCharge = await _walletServicesClient.Charge(requestChargeN);

            var balance = await _walletServicesClient.GetBalance(UserId);

            decimal amount = -1 * OriginBalance - 0.01m;

            var requestChargeLessThanN = _walletCharger.ChargeWallet(UserId, amount);

            var responce = await _walletServicesClient.Charge(requestChargeLessThanN);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(balance.Body, OriginBalance);
                Assert.AreEqual(HttpStatusCode.InternalServerError, responce.Status);
                Assert.AreEqual($"User have '{OriginBalance}.0', you try to charge '{amount.ToString().Replace(',', '.')}'.", responce.Content);
            });
        }



    }
}
