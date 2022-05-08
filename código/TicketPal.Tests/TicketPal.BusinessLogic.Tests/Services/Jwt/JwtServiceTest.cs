using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.BusinessLogic.Services;

namespace TicketPal.BusinessLogic.Tests.Services.Jwt
{
    [TestClass]
    public class JwtServiceTest : BaseServiceTest
    {
        private string secret;

        [TestInitialize]
        public void Init()
        {
            secret = "23jrb783v29fwfvfg2874gf286fce8";
            this.jwtService = new JwtService();
        }
        [TestMethod]
        public void ShouldTokenBeGenerated()
        {
            int id = 1234;
            string token = jwtService.GenerateJwtToken(secret, "id", id.ToString());

            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void GenerateTokenAndCheckIdDecrypted()
        {
            int id = 1234;
            var token = jwtService.GenerateJwtToken(secret, "id", id.ToString());


            var decrypted = jwtService.ClaimTokenValue(secret, token, "id");
            int decryptedId = int.Parse(decrypted);

            Assert.IsTrue(decryptedId == id);
        }
    }
}