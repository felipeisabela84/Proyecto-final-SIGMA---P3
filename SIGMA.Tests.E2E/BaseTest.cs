using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using NUnit.Framework;

namespace SIGMA.Tests.E2E
{
    public class BaseTest
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;
        protected const string BaseUrl = "https://localhost:7214"; // AJUSTA a tu puerto real

        protected ExtentTest Test;

        [SetUp]
        public void Configurar()
        {
            var opciones = new ChromeOptions();
            opciones.AddArgument("--ignore-certificate-errors");

            Driver = new ChromeDriver(opciones);
            Driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            Test = TestSetup.Extent.CreateTest(TestContext.CurrentContext.Test.FullName);
        }

        [TearDown]
        public void Limpiar()
        {
            var estado = TestContext.CurrentContext.Result.Outcome.Status;
            var nombrePrueba = TestContext.CurrentContext.Test.Name;

            string carpetaCapturas = Path.Combine(TestContext.CurrentContext.TestDirectory, "ReporteHTML", "Capturas");
            Directory.CreateDirectory(carpetaCapturas);
            string rutaCaptura = Path.Combine(carpetaCapturas, $"{nombrePrueba}.png");

            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            screenshot.SaveAsFile(rutaCaptura);

            if (estado == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Test.Fail("Prueba fallida", MediaEntityBuilder.CreateScreenCaptureFromPath(rutaCaptura).Build());
            }
            else
            {
                Test.Pass("Prueba exitosa", MediaEntityBuilder.CreateScreenCaptureFromPath(rutaCaptura).Build());
            }

            Driver.Quit();
            Driver.Dispose();
        }

        protected void IniciarSesion(string email, string password)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            Driver.FindElement(By.Id("Input_Email")).SendKeys(email);
            Driver.FindElement(By.Id("Input_Password")).SendKeys(password);
            Driver.FindElement(By.Id("login-submit")).Click();

            // Espera a que la navegación del login termine antes de continuar
            Wait.Until(d => !d.Url.Contains("/Login"));
        }
    }
}