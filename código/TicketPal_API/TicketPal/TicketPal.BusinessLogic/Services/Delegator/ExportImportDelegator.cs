using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using ExportImport;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Param;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.ExportImport;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;

namespace TicketPal.BusinessLogic.Services.Delegator
{
    public class ExportImportDelegator : IExportImportDelegator
    {
        private readonly IServiceFactory serviceFactory;
        private readonly IMapper mapper;
        public IGenericRepository<ConcertEntity> concertRepository;

        public ExportImportDelegator(IServiceFactory factory, IMapper mapper)
        {
            this.serviceFactory = factory;
            this.mapper = mapper;
            concertRepository = serviceFactory.GetRepository(typeof(ConcertEntity)) as IGenericRepository<ConcertEntity>;
        }

        public List<string> GetFormats()
        {
            List<string> formatsNames = new List<string>();
            IEnumerable<Type> implementations = GetAllTypes(typeof(IExportImport<>));
            foreach (var assembly in implementations)
            {
                formatsNames.Add(assembly.Name);
            }

            return formatsNames;
        }

        public IEnumerable<Type> GetAllTypes(Type genericType)
        {
            if (!genericType.IsGenericTypeDefinition)
                throw new ArgumentException("Specified type must be a generic type definition.", nameof(genericType));

            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                              i.GetGenericTypeDefinition().Equals(genericType)));
        }

        public OperationResult ExportImportConcerts(ExportImportParams param)
        {
            var implementation = GetAllTypes(typeof(IExportImport<>)).Where(i => i.Name == param.Format).FirstOrDefault();
            IExportImport<Concert> implementationInstance = (IExportImport<Concert>)Activator.CreateInstance(implementation);
            
            if (param.Action == Constants.EXPORT)
            {
                try
                {
                    var concerts = mapper.Map<List<Concert>>(concertRepository.GetAll());
                    implementationInstance.Export(Constants.EXPORT_PATH, concerts);
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_SUCCESS,
                        Message = "Concerts exported successfully"
                    };
                }
                catch (RepositoryException ex)
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                try
                {
                    var importedConcerts = implementationInstance.Import(Constants.IMPORT_PATH);
                    foreach (var concert in importedConcerts)
                    {
                        var newConcert = mapper.Map<ConcertEntity>(concert);
                        concertRepository.Add(newConcert);
                    }
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_SUCCESS,
                        Message = "Concerts imported successfully"
                    };
                }
                catch (RepositoryException ex)
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = ex.Message
                    };
                }
            }
        }
    }
}
