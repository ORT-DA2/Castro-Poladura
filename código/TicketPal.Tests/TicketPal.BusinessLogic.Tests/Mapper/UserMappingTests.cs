using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Tests.Mapper
{
    [TestClass]
    public class UserMappingTests
    {
        [TestMethod]
        public void UserEntityToUserMapperTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapping>());
            var mapper = config.CreateMapper();

            var toBeMapped = new UserEntity
            {
                Id = 1,
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "SomePassword",
                Role = UserRole.ADMIN.ToString()
            };

            var user = mapper.Map<User>(toBeMapped);

            Assert.IsNotNull(user);
            Assert.IsTrue(user.Email.Equals(toBeMapped.Email)
                && user.Firstname.Equals(toBeMapped.Firstname)
                && user.Lastname.Equals(toBeMapped.Lastname)
                && user.Id == toBeMapped.Id
                && user.Password.Equals(toBeMapped.Password)
                && user.Role.ToString().Equals(toBeMapped.Role.ToString())
            );
        }
    }
}