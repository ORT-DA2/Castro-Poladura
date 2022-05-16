
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
                        Role = Constants.ROLE_ADMIN
                    },
                    new UserEntity
                    {
                        Id = 2,
                        Firstname = "Ricardo",
                        Lastname = "Poladura",
                        Email = "ricardo@example.com",
                        Password = BC.HashPassword("ricardo1"),
                        Role = Constants.ROLE_ADMIN
                    },
                    new UserEntity
                    {
                        Id = 3,
                        Firstname = "Spectator",
                        Lastname = "Test",
                        Email = "spectator@example.com",
                        Password = BC.HashPassword("spectator1"),
                        Role = Constants.ROLE_SPECTATOR
                    },
                    new UserEntity
                    {
                        Id = 4,
                        Firstname = "Seller",
                        Lastname = "Test",
                        Email = "seller@example.com",
                        Password = BC.HashPassword("seller1"),
                        Role = Constants.ROLE_SELLER
                    },
                    new UserEntity
                    {
                        Id = 5,
                        Firstname = "Supervisor",
                        Lastname = "Test",
                        Email = "supervisor@example.com",
                        Password = BC.HashPassword("supervisor1"),
                        Role = Constants.ROLES_SUPERVISOR
                    },
                    new UserEntity
                    {
                        Id = 6,
                        Firstname = "Artist",
                        Lastname = "Test",
                        Email = "artist@example.com",
                        Password = BC.HashPassword("artist1"),
                        Role = Constants.ROLE_ARTIST
                    }
                };
            }
        }
    }
}