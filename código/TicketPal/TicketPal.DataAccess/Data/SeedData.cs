
using System.Collections.Generic;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using BC = BCrypt.Net.BCrypt;

namespace TicketPal.DataAccess.Data
{
    public class SeedData
    {
        public static List<UserEntity> Users
        {
            get
            {
                return new List<UserEntity>
                {
                    new UserEntity
                    {
                        Id = 1,
                        Firstname = "Lucas",
                        Lastname = "Castro",
                        Email = "lucas@example.com",
                        Password = BC.HashPassword("lucas1"),
                        Role = UserRole.ADMIN.ToString()
                    },
                    new UserEntity
                    {
                        Id = 2,
                        Firstname = "Ricardo",
                        Lastname = "Poladura",
                        Email = "ricardo@example.com",
                        Password = BC.HashPassword("ricardo1"),
                        Role = UserRole.ADMIN.ToString()
                    },
                    new UserEntity
                    {
                        Id = 3,
                        Firstname = "Spectator",
                        Lastname = "Test",
                        Email = "spectator@example.com",
                        Password = BC.HashPassword("spectator1"),
                        Role = UserRole.SPECTATOR.ToString()
                    },
                    new UserEntity
                    {
                        Id = 4,
                        Firstname = "Seller",
                        Lastname = "Test",
                        Email = "seller@example.com",
                        Password = BC.HashPassword("seller1"),
                        Role = UserRole.SELLER.ToString()
                    },
                    new UserEntity
                    {
                        Id = 5,
                        Firstname = "Supervisor",
                        Lastname = "Test",
                        Email = "supervisor@example.com",
                        Password = BC.HashPassword("supervisor1"),
                        Role = UserRole.SUPERVISOR.ToString()
                    }
                };
            }
        }
    }
}