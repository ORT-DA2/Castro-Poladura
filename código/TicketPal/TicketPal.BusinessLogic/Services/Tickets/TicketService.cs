using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Tickets;

namespace TicketPal.BusinessLogic.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository repository;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;

        public TicketService(ITicketRepository repository, IOptions<IAppSettings> appSettings, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.appSettings = appSettings.Value;
        }

        public OperationResult AddTicket(AddTicketRequest model)
        {
            throw new NotImplementedException();
        }

        public OperationResult DeleteTicket(int id)
        {
            throw new NotImplementedException();
        }

        public bool ExistsTicketByName(string name)
        {
            throw new NotImplementedException();
        }

        public Ticket GetTicket(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ticket> GetTickets()
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdateTicket(UpdateTicketRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
