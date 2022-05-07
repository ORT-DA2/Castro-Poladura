using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.Factory.Tests
{
    [TestClass]
    public class BaseTestFactoryConfig
    {
        protected IServiceCollection services;

        [TestInitialize]
        public void SetUp()
        {
            this.services = new ServiceCollection();
        }
    }
}