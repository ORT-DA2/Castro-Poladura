using System;

namespace TicketPal.Interfaces.Factory
{
    public interface IServiceFactory
    {
        void AddDbContextService(string connectionString);
        void RegisterRepositories();
        void RegisterServices();
        void BuildServices();
        object GetRepository(Type classType);
        object GetService(Type classType);
    }
}