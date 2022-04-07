using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public abstract class RepositoryBaseConfigTest
    {
        protected ServiceCollection Services { get; private set; }
        protected ServiceProvider ServiceProvider { get; private set; }
        protected AppDbContext dbContext;

        [TestInitialize]
        public void Initialize()
        {
            Services = new ServiceCollection();

            Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped);

            ServiceProvider = Services.BuildServiceProvider();
            dbContext = ServiceProvider.GetService<AppDbContext>();
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            dbContext.Database.EnsureDeleted();

            ServiceProvider.Dispose();
            ServiceProvider = null;
        }
    }
}