using System;
using Moq;

namespace SimonsSearch.Testing
{
    public class BaseTest : IDisposable
    {
        protected BaseTest()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
        }

        protected MockRepository MockRepository { get; }

        public void Dispose()
        {
            MockRepository.VerifyAll();
        }
    }
}