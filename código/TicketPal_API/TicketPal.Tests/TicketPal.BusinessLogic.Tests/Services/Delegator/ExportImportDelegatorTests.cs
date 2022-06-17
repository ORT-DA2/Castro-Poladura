using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExportImport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Services.Delegator;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Param;
using TicketPal.Domain.Models.Response;
using TicketPal.Factory;
using TicketPal.Interfaces.ExportImport;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.WebApi.Controllers;

namespace TicketPal.BusinessLogic.Tests.Services.Delegator
{
    [TestClass]
    public class ExportImportDelegatorTests: BaseServiceTests
    {
        private Mock<IExportImportDelegator> delegatorMock;
        private ExportImportDelegator delegator;
        private List<PerformerEntity> performers;
        private IEnumerable<ConcertEntity> concerts;
        private UserEntity user;
        private PerformerEntity performer;
        private ConcertEntity concert;

        [TestInitialize]
        public void Initialize()
        {
            /*performer = new PerformerEntity()
            {
                CreatedAt = DateTime.Now,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    Name = "Pop"
                },
                Id = 2,
                Members = new List<PerformerEntity>(),
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "1950",
                UpdatedAt = DateTime.Now,
            };

            user = new UserEntity()
            {
                ActiveAccount = true,
                CreatedAt = DateTime.Now,
                Email = "example@example.com",
                Firstname = "Pablo",
                Lastname = "Marmol",
                Id = 1,
                Password = "owi4hfnsdkj",
                Performer = performer,
                Role = Constants.ROLE_ARTIST,
                UpdatedAt = DateTime.Now
            };

            performer.UserInfo = user;

            performers = new List<PerformerEntity>();
            performers.Add(performer);

            concert = new ConcertEntity()
            {
                Address = "Calle 1234",
                Artists = performers,
                AvailableTickets = 1000,
                Country = "Uruguay",
                CreatedAt = DateTime.Now,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                Date = new DateTime(2022-08-27),
                EventType = Constants.EVENT_CONCERT_TYPE,
                Id = 234,
                Location = "Estadio Nuevo",
            };
            concerts = new List<ConcertEntity>()
            {
                concert
            };

            this.delegatorMock.Setup(x => x.GetAllTypes(typeof(IExportImport<>))).Returns(implementations);
            this.mockConcertRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<ConcertEntity, bool>>>())).Returns(Task.FromResult(concerts.ToList()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            delegator = new ExportImportDelegator(factoryMock.Object, this.mapper);*/
        }

        [TestMethod]
        public void GetFormatsSuccessfullyTest()
        {

        }

        [TestMethod]
        public void GetAllTypesSuccessfullyTest()
        {

        }

        [TestMethod]
        public void ExportConcertsSuccessfullyTest()
        {
            /*OkObjectResult expectedResult = new OkObjectResult(null)
            {
                Value = "",
                StatusCode = 200
            };
            var result = delegator.ExportImportConcerts(new ExportImportParams()
            {
                Action = "EXPORT",
                Format = "JSON Format"
            });
            Assert.AreEqual(expectedResult, result);*/
        }

        [TestMethod]
        public void ImportConcertsSuccessfullyTest()
        {
            /*OkObjectResult expectedResult = new OkObjectResult(null)
            {
                Value = "",
                StatusCode = 200
            };
            var result = delegator.ExportImportConcerts(new ExportImportParams()
            {
                Action = "IMPORT",
                Format = "JSON Format"
            });
            Assert.AreEqual(expectedResult, result);*/
        }
    }
}
