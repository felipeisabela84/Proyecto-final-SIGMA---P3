using OpenQA.Selenium;
using NUnit.Framework;

namespace SIGMA.Tests.E2E
{
    public class LoginTests : BaseTest
    {
        [Test] // Camino feliz
        public void Login_CiudadanoConCredencialesValidas_RedirigeCorrectamente()
        {
            IniciarSesion("ciudadano@test.com", "Ciudadano1@");
            Assert.That(Driver.PageSource, Does.Contain("Ciudadano")); 
        }

        [Test] // Camino feliz
        public void Login_OperadorConCredencialesValidas_RedirigeCorrectamente()
        {
            IniciarSesion("operador@test.com", "Operador1@");
            Assert.That(Driver.PageSource, Does.Contain("Operador")); 
        }

        [Test] // Negativo
        public void Login_ConPasswordIncorrecta_MuestraError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            Driver.FindElement(By.Id("Input_Email")).SendKeys("ciudadano@test.com");
            Driver.FindElement(By.Id("Input_Password")).SendKeys("PasswordIncorrecta123!");
            Driver.FindElement(By.Id("login-submit")).Click();

            Wait.Until(d => d.PageSource.Contains("Invalid login attempt"));

            Assert.That(Driver.PageSource, Does.Contain("Invalid login attempt"));
        }
    }
}