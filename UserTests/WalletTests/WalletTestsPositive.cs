using NUnit.Framework;
using System.Net;
using UserTests.Clients;
using UserTests.Utils;
using WalletTests.Clients;

namespace UserTests.WalletTests
{
    [TestFixture]
    public class WalletTestsPositive
    {
        private readonly WalletServiceClient _walletServicesClient = new WalletServiceClient();
        private readonly WalletCharger _walletCharger = new WalletCharger();
        private readonly UserServiceClient _userServiceClient = UserServiceClient.Instance;
        private readonly UserGenerator _userGenerator = new UserGenerator();


        [Test]
        public async Task GetBalance_NoTransactions_StatusCodeIsOk()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            Console.WriteLine(responceRegister.Body);

            var requestStatus = await _userServiceClient.UpdateUser(int.Parse(responceRegister.Content), true);

            var responceGetBalance = await _walletServicesClient.GetBalance(int.Parse(responceRegister.Content));

            var responceTransactions = await _walletServicesClient.GetTransactions(int.Parse(responceRegister.Content));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceGetBalance.Status);
                Assert.AreEqual(0, responceTransactions.BodyArr.Length);
                Assert.AreEqual(0, responceGetBalance.Body);
            });
        }

        [Test]
        public async Task GetBalance_MakeBalanceRequestAfterRevert_StatusCodeIsOk()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            int UserId = int.Parse(responceRegister.Content);

            var responceSetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestCharge = _walletCharger.ChargeWallet(UserId, 25000);

            var responceGetBalance = await _walletServicesClient.Charge(requestCharge);

            var responceGetTransactions = await _walletServicesClient.GetTransactions(UserId);

            var balanceRequest = await _walletServicesClient.GetBalance(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceGetTransactions.Status);
                Assert.AreEqual(HttpStatusCode.OK, balanceRequest.Status);
            });
        }

        [Test]
        public async Task GetBalance_OneTransaction_OveralBalance_ZeroDotZeroOne_StatusCodeIsOk ()
        {
            decimal amount = 0.01m;

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
                Assert.AreEqual(HttpStatusCode.OK,responceRegister.Status);
                Assert.AreEqual(HttpStatusCode.OK,responceGetBalance.Status);
                Assert.AreEqual(amount, balanceRequest.Body);
                Assert.AreEqual(1, responceGetTransactions.BodyArr.Length);

            });
        }


        [Test]
        public async Task GetTransaction_OneTransaction_StatusCodeIsOk()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            int UserId = int.Parse(responceRegister.Content);

            var responceSetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestCharge = _walletCharger.ChargeWallet(UserId, 25000);

            var responceGetBalance = await _walletServicesClient.Charge(requestCharge);

            var responceGetTransactions = await _walletServicesClient.GetTransactions(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceRegister.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceSetStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceGetBalance.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceGetTransactions.Status);
                Assert.AreEqual(responceGetTransactions.BodyArr.Length, 1);
            });
        }

        [Test]
        public async Task GetTransaction_MakeGetTransactionsRequestAfterRevert_StatusCodeIsOk()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            int UserId = int.Parse(responceRegister.Content);

            var responceSetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestCharge = _walletCharger.ChargeWallet(UserId, 25000);

            var responceGetBalance = await _walletServicesClient.Charge(requestCharge);

            var tID = responceGetBalance.Content.Substring(1, responceGetBalance.Content.Length - 2);

            var responceRevertTransaction = await _walletServicesClient.RevertTransaction(tID);

            var responceGetTransactions = await _walletServicesClient.GetTransactions(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceRevertTransaction.Status);
                Assert.AreEqual(tID, responceGetTransactions.BodyArr.First().BaseTransactionId); ;
            });
        }

        [Test]
        public async Task GetTransaction_NoTransaction_StatusCodeIsOk()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            int UserId = int.Parse(responceRegister.Content);

            var responceGetTransactions = await _walletServicesClient.GetTransactions(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceRegister.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceGetTransactions.Status);
                Assert.AreEqual(0, responceGetTransactions.BodyArr.Length);
            });
        }

        [Test]
        public async Task RevertTransaction_AmountEqualTo10M_StatusCodeIsOk()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            int UserId = int.Parse(responceRegister.Content);

            var responceSetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestCharge = _walletCharger.ChargeWallet(UserId, 10000000);

            var responceCharge = await _walletServicesClient.Charge(requestCharge);

            var tID = responceCharge.Content.Substring(1, responceCharge.Content.Length - 2);

            var responceRevertTransaction = await _walletServicesClient.RevertTransaction(tID);

            var lastTransaction = await _walletServicesClient.GetTransactions(UserId);

            var balanceRequest = await _walletServicesClient.GetBalance(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceRegister.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceSetStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceCharge.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceRevertTransaction.Status);
                Assert.AreEqual(HttpStatusCode.OK, lastTransaction.Status);
                Assert.AreEqual(lastTransaction.BodyArr.First().TransactionId, responceRevertTransaction.Body);
                Assert.AreEqual(lastTransaction.BodyArr.First().Amount, -10000000);
                Assert.AreEqual(0, balanceRequest.Body);
            });

        }

        [Test]
        public async Task RevertTransaction_AmountEqualToZeroDotZeroOne_StatusCodeIsOk()
        {
            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var request = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(request);

            int UserId = int.Parse(responceRegister.Content);

            var responceSetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestCharge = _walletCharger.ChargeWallet(UserId, 0.01m);

            var responceCharge = await _walletServicesClient.Charge(requestCharge);

            var tID = responceCharge.Content.Substring(1, responceCharge.Content.Length - 2);

            var responceRevertTransaction = await _walletServicesClient.RevertTransaction(tID);

            var lastTransaction = await _walletServicesClient.GetTransactions(UserId);

            var balanceRequest = await _walletServicesClient.GetBalance(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceRegister.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceSetStatus.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceCharge.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceRevertTransaction.Status);
                Assert.AreEqual(HttpStatusCode.OK, lastTransaction.Status);
                Assert.AreEqual(lastTransaction.BodyArr.First().TransactionId, responceRevertTransaction.Body);
                Assert.AreEqual(lastTransaction.BodyArr.First().Amount, -0.01);
                Assert.AreEqual(0, balanceRequest.Body);
            });

        }

        [Test]
        public async Task Charge_BalanceNChargeN_StatusCodeIsOk()
        {
            decimal OriginAmount = 1500m;

            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var requestRegister = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(requestRegister);

            int UserId = int.Parse(responceRegister.Content);

            var responceGetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestChargeN = _walletCharger.ChargeWallet(UserId, OriginAmount);

            var responceCharge = await _walletServicesClient.Charge(requestChargeN);

            var balance = await _walletServicesClient.GetBalance(UserId);

            decimal amount = -1 * OriginAmount;

            var requestChargeLessThanN = _walletCharger.ChargeWallet(UserId, amount);

            var responce = await _walletServicesClient.Charge(requestChargeLessThanN);

            balance = await _walletServicesClient.GetBalance(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responce.Status);
                Assert.AreEqual(OriginAmount + amount, balance.Body);
            });

        }

        [Test]
        public async Task Charge_BalanceNPlusTenChargeMinusN_StatusCodeIsOk()
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

            decimal amount = OriginBalance - 10m;

            var requestChargeLessThanN = _walletCharger.ChargeWallet(UserId, amount);

            var responce = await _walletServicesClient.Charge(requestChargeLessThanN);

            balance = await _walletServicesClient.GetBalance(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responce.Status);
                Assert.AreEqual(balance.Body, OriginBalance + amount);
            });

        }

        [Test]
        public async Task Charge_Balance_ZeroDotZeroOne_StatusCodeIsOk()
        {
            decimal amount = 0.01m;

            string name = _userGenerator.GetRandomString(5);

            string surname = _userGenerator.GetRandomString(5);

            var requestRegister = _userGenerator.GenerateRegisterUserRequest(name, surname);

            var responceRegister = await _userServiceClient.RegisterUser(requestRegister);

            int UserId = int.Parse(responceRegister.Content);

            var responceGetStatus = await _userServiceClient.UpdateUser(UserId, true);

            var requestChargeN = _walletCharger.ChargeWallet(UserId, amount);

            var responceCharge = await _walletServicesClient.Charge(requestChargeN);

            var balance = await _walletServicesClient.GetBalance(UserId);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, responceRegister.Status);
                Assert.AreEqual(HttpStatusCode.OK, responceCharge.Status);
                Assert.AreEqual(amount, balance.Body);
            });


        }


    }
}
