using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;
using TicketPal.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace TicketPal.DataAccess.Repository
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private bool disposed;
        protected readonly DbContext dbContext;
        public GenericRepository(DbContext context)
        {
            dbContext = context;
        }

        public virtual void Add(TEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The element that is being added already exists");
            }

            dbContext.Set<TEntity>().Add(element);
            element.CreatedAt = DateTime.Now;
            dbContext.SaveChanges();
        }

        public void Clear()
        {
            dbContext.Set<TEntity>().RemoveRange(dbContext.Set<TEntity>());
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            TEntity toDelete = Get(id);

            if (toDelete == null)
            {
                throw new RepositoryException(string.Format("The element with id: {0} doesn't exist", id));
            }

            dbContext.Set<TEntity>().Remove(toDelete);
            dbContext.SaveChanges();
        }

        public virtual bool Exists(int id)
        {
            return dbContext.Set<TEntity>().Any(item => item.Id == id);
        }

        public TEntity Get(int id)
        {
            return dbContext.Set<TEntity>().FirstOrDefault(u => u.Id == id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>()
                .Where(predicate)
                .AsEnumerable();
        }

        public bool IsEmpty()
        {
            return !dbContext.Set<TEntity>().Any();
        }

        public abstract void Update(TEntity element);


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}