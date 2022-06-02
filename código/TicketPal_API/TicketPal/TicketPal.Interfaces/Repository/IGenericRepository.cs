using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TicketPal.Interfaces.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity element);
        void Update(TEntity element);
        bool IsEmpty();
        bool Exists(int id);
        Task<TEntity> Get(int id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);
        void Delete(int id);
        void Clear();
    }
}