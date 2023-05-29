using NUnit.Framework;
using System.Collections.Concurrent;
using UserTests.Clients;

namespace UserTests
{
    [SetUpFixture]
    public class Storage
    {

        private TestDataObserver _observer;
        private UserServiceClient _client = UserServiceClient.Instance;

        [OneTimeSetUp]
        public void OneTimeSetUp ()
        {
            _observer = new TestDataObserver();
            _client.Subscribe(_observer);
        }


        [OneTimeTearDown]
        public async Task OneTimeTearDown ()
        {
            var tasks = _observer
                .GetAllUsers()
                .Select(id => _client.DeleteUser(id));

            await Task.WhenAll(tasks);
        }

        public class TestDataObserver : IObserver<int>
        {

            private readonly ConcurrentBag<int> _storage = new ConcurrentBag<int>();

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                _storage.Add(value);
            }

            public IEnumerable<int> GetAllUsers ()
            {
                return _storage.ToArray();
            }

        }
    }
}
