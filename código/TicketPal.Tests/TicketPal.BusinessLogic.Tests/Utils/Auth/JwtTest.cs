using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.BusinessLogic.Tests.Utils.Auth
{
    public class JwtTest
    {
        private string secret;

        [TestInitialize]
        public void Init()
        {
            secret = "23jrb783v29fwfvfg2874gf286fce8";
        }
        [TestMethod]
        public void ShouldTokenBeGenerated()
        {
            int id = 1234;
            string token = JwtUtils.GenerateJwtToken(secret,"id",id.ToString());

            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void GenerateTokenAndCheckIdDecrypted()
        {
            int id = 1234;
            var token = JwtUtils.GenerateJwtToken(secret, "id", id.ToString());

            
            var decrypted = JwtUtils.ClaimTokenValue(secret,token,"id");
            int decryptedId = int.Parse(decrypted);

            Assert.IsTrue(decryptedId == id);
        }
    }
}