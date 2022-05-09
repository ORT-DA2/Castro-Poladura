using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.BusinessLogic.Services.Settings;

namespace TicketPal.BusinessLogic.Tests.Settings
{
    [TestClass]
    public class AppSettingsTest
    {
        [TestMethod]
        public void TestJwtSecretNotNull()
        {
            AppSettings settings = new AppSettings();
            settings.JwtSecret = "myTestJwtSecret";
            Assert.IsNotNull(settings.JwtSecret);
        }
    }
}