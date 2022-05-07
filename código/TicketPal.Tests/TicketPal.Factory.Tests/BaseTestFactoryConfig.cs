using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TicketPal.Factory.Tests
{
    [TestClass]
    public class BaseTestFactoryConfig
    {
        protected IServiceCollection services;
        protected Mock<IConfiguration> mockConfig;
        protected Mock<IMapper> mockMapper;
        protected string testConnectionString = "SomeFakeConnectionString";

        [TestInitialize]
        public void SetUp()
        {
            this.services = new ServiceCollection();
            this.mockConfig = new Mock<IConfiguration>();

            Mock<IConfigurationSection> configurationSectionStub = new Mock<IConfigurationSection>();
            configurationSectionStub.Setup(x => x[It.IsAny<string>()]).Returns(testConnectionString);
            this.mockConfig = new Mock<IConfiguration>();
            this.mockConfig.Setup(x => x.GetSection(It.IsAny<string>())).Returns(configurationSectionStub.Object);
        }
    }
}