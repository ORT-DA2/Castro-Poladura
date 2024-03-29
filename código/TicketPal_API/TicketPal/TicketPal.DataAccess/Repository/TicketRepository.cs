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
    public class TicketRepository : GenericRepository<TicketEntity>
    {
        public TicketRepository(DbContext context) : base(context)
        {
        }

        public override async Task Add(TicketEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The ticket you are trying to add already exists");
            }

            var containedEvent = dbContext.Set<EventEntity>().FirstOrDefault(e => e.Id == element.Event.Id);

            if (containedEvent != null)
            {
                if (containedEvent.AvailableTickets > 0)
                {
                    if (element.Buyer != null && element.Buyer.Id != 0)
                    {
                        dbContext.Attach(element.Buyer);
                    }
                    containedEvent.AvailableTickets = containedEvent.AvailableTickets - 1;
                    dbContext.Entry(containedEvent).State = EntityState.Modified;
                    element.Event = containedEvent;

                    await dbContext.Set<TicketEntity>().AddAsync(element);
                    element.CreatedAt = DateTime.Now;
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new RepositoryException("No available tickets for this event.");
                }
            }
            else
            {
                throw new RepositoryException("No events found for this purchase.");
            }

        }

        public override void Update(TicketEntity element)
        {
            var found = dbContext.Set<TicketEntity>().FirstOrDefault(t => t.Id == element.Id);

            if (found == null)
            {
                throw new RepositoryException(string.Format("Couldn't find item to update with id: {0} doesn't exist", element.Id));
            }

            found.Buyer = (element.Buyer == null ? found.Buyer : element.Buyer);
            found.Event = (element.Event == null ? found.Event : element.Event);
            found.PurchaseDate = element.PurchaseDate;
            found.Code = (element.Code == null ? found.Code : element.Code);
            found.Status = (element.Status == null ? found.Status : element.Status);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }

        public async override Task<TicketEntity> Get(int id)
        {
            return await dbContext.Set<TicketEntity>()
            .Include(t => t.Buyer)
            .Include(t => t.Event)
            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async override Task<TicketEntity> Get(Expression<Func<TicketEntity, bool>> predicate)
        {
            return await dbContext.Set<TicketEntity>()
            .Include(t => t.Buyer)
            .Include(t => t.Event)
            .FirstOrDefaultAsync(predicate);
        }

        public async override Task<List<TicketEntity>> GetAll()
        {
            return await dbContext.Set<TicketEntity>()
            .Include(t => t.Buyer)
            .Include(t => t.Event)
            .ToListAsync();
        }

        public async override Task<List<TicketEntity>> GetAll(Expression<Func<TicketEntity, bool>> predicate)
        {
            return await dbContext.Set<TicketEntity>()
            .Include(t => t.Buyer)
            .Include(t => t.Event)
            .Where(predicate)
            .ToListAsync();
        }
    }
}
