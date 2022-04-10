// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using System;

// namespace TicketPal.DataAccess.Tests.Entity
// {
//     [TestClass]
//     public class EventEntityTests
//     {
//         private EventEntity eventBase;
//         private int idEvent;
//         private DateTime eventDate;
//         private int availableTickets;
//         private decimal ticketPrice;
//         private int Id;
//         private DateTime createdAt;
//         private DateTime updatedAt;


//         [TestInitialize]
//         public void Initialize()
//         {
//             Id = 1;
//             createdAt = new DateTime(2022, 04, 06);
//             updatedAt = new DateTime(2022, 04, 07);
//             eventBase = new EventEntity();
//             idEvent = 1;
//             eventDate = new DateTime(2022, 05, 16);
//             availableTickets = 500;
//             ticketPrice = 2350.00m;
//         }

//         [TestMethod]
//         public void SetEventTest()
//         {
//             eventBase.IdEvent = idEvent;
//             eventBase.EventDate = eventDate;
//             eventBase.AvailableTickets = availableTickets;
//             eventBase.TicketPrice = ticketPrice;

//             Assert.AreEqual(eventBase.IdEvent, idEvent);
//             Assert.AreEqual(eventBase.EventDate, eventDate);
//             Assert.AreEqual(eventBase.AvailableTickets, availableTickets);
//             Assert.AreEqual(eventBase.TicketPrice, ticketPrice);
//         }

//         [TestMethod]
//         public void GetEventTest()
//         {
//             int idEvent = eventBase.IdEvent;
//             DateTime eventDate = eventBase.EventDate;
//             int availableTickets = eventBase.AvailableTickets;
//             decimal ticketPrice = eventBase.TicketPrice;

//             Assert.AreEqual(eventBase.IdEvent, idEvent);
//             Assert.AreEqual(eventBase.EventDate, eventDate);
//             Assert.AreEqual(eventBase.AvailableTickets, availableTickets);
//             Assert.AreEqual(eventBase.TicketPrice, ticketPrice);
//         }
//     }
// }
