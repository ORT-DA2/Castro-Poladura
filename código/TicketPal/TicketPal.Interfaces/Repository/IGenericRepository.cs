using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TicketPal.Interfaces.Repository
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity element);
        void Update(TEntity element);
        bool IsEmpty();
        bool Exists(int id);
        TEntity Get(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        void Delete(int id);
        void Clear();
    }
}