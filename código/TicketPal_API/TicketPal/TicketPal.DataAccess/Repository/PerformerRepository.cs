﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Repository
{
    public class PerformerRepository : GenericRepository<PerformerEntity>
    {
        public PerformerRepository(DbContext context) : base(context)
        {
        }

        public override async Task Add(PerformerEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The performer you are trying to add already exists");
            }

            await dbContext.Set<PerformerEntity>().AddAsync(element);
            element.CreatedAt = DateTime.Now;
            dbContext.SaveChanges();
        }

        public override void Update(PerformerEntity element)
        {
            var found = dbContext.Set<PerformerEntity>().FirstOrDefault(p => p.Id == element.Id);

            if (found == null)
            {
                throw new RepositoryException(string.Format("Couldn't find item to update with id: {0} doesn't exist", element.Id));
            }

            found.UserInfo = (element.UserInfo == null ? found.UserInfo : element.UserInfo);
            found.PerformerType = (element.PerformerType == null ? found.PerformerType : element.PerformerType);
            found.StartYear = (element.StartYear == null ? found.StartYear : element.StartYear);
            found.Members = (element.Members == null ? found.Members : element.Members);
            found.Genre = (element.Genre == null ? found.Genre : element.Genre);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }

        public async override Task<PerformerEntity> Get(int id)
        {
            return await dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Members)
            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async override Task<PerformerEntity> Get(Expression<Func<PerformerEntity, bool>> predicate)
        {
            return await dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Members)
            .FirstOrDefaultAsync(predicate);
        }

        public async override Task<List<PerformerEntity>> GetAll()
        {
            return await dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Members)
            .ToListAsync();
        }

        public async override Task<List<PerformerEntity>> GetAll(Expression<Func<PerformerEntity, bool>> predicate)
        {
            return await dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Members)
            .Where(predicate)
            .ToListAsync();
        }
    }
}
