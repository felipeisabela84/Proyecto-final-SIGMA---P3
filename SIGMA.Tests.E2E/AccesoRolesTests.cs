using OpenQA.Selenium;
using NUnit.Framework;

namespace SIGMA.Tests.E2E
{
    public class AccesoRolesTests : BaseTest
    {
        [Test]
        public void Ciudadano_IntentaAccederAlPanelOperador_EsDenegado()
        {
            IniciarSesion("ciudadano@test.com", "Ciudadano1@");

            Driver.Navigate().GoToUrl($"{BaseUrl}/Operador/Reportes");

            Wait.Until(d => d.Url.Contains("AccessDenied"));

            Assert.That(Driver.Url, Does.Contain("AccessDenied"));
            Assert.That(Driver.PageSource, Does.Contain("Access denied"));
        }
    }
}