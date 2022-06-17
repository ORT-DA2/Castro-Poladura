using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;
using TicketPal.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TicketPal.DataAccess.Repository
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext dbContext;
        public GenericRepository(DbContext context)
        {
            dbContext = context;
        }

        public virtual async Task Add(TEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The element that is being added already exists");
            }

            await dbContext.Set<TEntity>().AddAsync(element);
            element.CreatedAt = DateTime.Now;
            dbContext.SaveChanges();
        }

        public void Clear()
        {
            dbContext.Set<TEntity>().RemoveRange(dbContext.Set<TEntity>());
            dbContext.SaveChanges();
        }

        public async Task Delete(int id)
        {
            TEntity toDelete = await Get(id);

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

        public async virtual Task<TEntity> Get(int id)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async virtual Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async virtual Task<List<TEntity>> GetAll()
        {
            return await dbContext.Set<TEntity>().ToListAsync();
        }

        public async virtual Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }

        public bool IsEmpty()
        {
            return !dbContext.Set<TEntity>().Any();
        }

        public abstract void Update(TEntity element);
    }
}