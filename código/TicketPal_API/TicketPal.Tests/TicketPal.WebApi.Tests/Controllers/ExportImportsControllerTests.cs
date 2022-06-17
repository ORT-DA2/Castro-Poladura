using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExportImport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Param;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.ExportImport;
using TicketPal.WebApi.Controllers;

namespace TicketPal.WebApi.Tests.Controllers
{
    [TestClass]
    public class ExportImportsControllerTests
    {
        Mock<IExportImportDelegator> exportDelegatorMock;
        private ExportsImportsController controller;
        private ExportImportParams param;
        private List<string> assemblyList;

        [TestInitialize]
        public void Initialize()
        {
            assemblyList = new List<string>();
            assemblyList.Add("JSON Format");
            assemblyList.Add("Xml Format");

            param = new ExportImportParams()
            {
                Action = "EXPORT",
                Format = "JSON Format"
            };

            exportDelegatorMock = new Mock<IExportImportDelegator>();

            exportDelegatorMock.Setup(x => x.GetFormats()).Returns(assemblyList);
            exportDelegatorMock.Setup(x => x.ExportImportConcerts(param));
            controller = new ExportsImportsController(exportDelegatorMock.Object);
        }

        [TestMethod]
        public void GetFormatsSuccessfullyTest()
        {
            OkObjectResult expectedResult = new OkObjectResult(null)
            {
                Value = assemblyList,
                StatusCode = 200
            };
            var result = controller.GetFormats();
            var objectResult = result as ObjectResult;
            Assert.AreEqual(expectedResult.StatusCode, objectResult.StatusCode);
        }

        [TestMethod]
        public void ExportImportSuccessfullyTest()
        {
            assemblyList = new List<string>();
            assemblyList.Add("JSON Format");
            OkObjectResult expectedResult = new OkObjectResult(null)
            {
                Value = assemblyList,
                StatusCode = 200
            };
            var result = controller.ExportImportConcerts(param);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(expectedResult.StatusCode, objectResult.StatusCode);
        }
    }
}
