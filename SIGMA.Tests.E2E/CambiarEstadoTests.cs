using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace SIGMA.Tests.E2E
{
    public class CambiarEstadoTests : BaseTest
    {
        private void CrearReporteDePrueba(string zona)
        {
            IniciarSesion("ciudadano@test.com", "Ciudadano1@");
            Driver.Navigate().GoToUrl($"{BaseUrl}/Reportes/Crear");

            Driver.FindElement(By.Id("Zona")).SendKeys(zona);
            ((IJavaScriptExecutor)Driver).ExecuteScript(
                "arguments[0].value = '2026-08-15T10:00';",
                Driver.FindElement(By.Id("FechaHora")));
            Driver.FindElement(By.Id("DuracionMinutos")).SendKeys("30");

            Driver.FindElement(By.CssSelector("form[action='/Reportes/Crear'] button[type='submit']")).Click();
            Wait.Until(d => d.Url.Contains("MisReportes"));

            // Cierra sesión del Ciudadano antes de que el Operador inicie la suya
            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Logout");
        }

        [Test] // Camino feliz
        public void Operador_CambiaEstadoDeReporte_SeActualizaCorrectamente()
        {
            CrearReporteDePrueba("Zona Test Estado");

            IniciarSesion("operador@test.com", "Operador1@"); // AJUSTA al password real del Operador
            Driver.Navigate().GoToUrl($"{BaseUrl}/Operador/Reportes");

            var fila = Driver.FindElement(By.XPath("//tr[td[contains(text(),'Zona Test Estado')]]"));
            var select = fila.FindElement(By.CssSelector("select[name='nuevoEstado']"));
            new SelectElement(select).SelectByText("EnInvestigacion");

            fila.FindElement(By.CssSelector("button[type='submit']")).Click();

            Wait.Until(d => d.PageSource.Contains("EnInvestigacion"));

            Assert.That(Driver.PageSource, Does.Contain("Zona Test Estado"));
        }

        [Test] // Negativo — Ciudadano no debería ver este formulario en absoluto
        public void Ciudadano_NoVeFormularioDeCambiarEstado()
        {
            IniciarSesion("ciudadano@test.com", "Ciudadano1@");
            Driver.Navigate().GoToUrl($"{BaseUrl}/Reportes/MisReportes");

            var formulariosCambiarEstado = Driver.FindElements(By.CssSelector("form[action*='CambiarEstado']"));

            Assert.That(formulariosCambiarEstado.Count, Is.EqualTo(0));
        }

        [Test] // Límite — cambiar al mismo estado no debe romper nada
        public void Operador_CambiaAlMismoEstado_NoGeneraError()
        {
            CrearReporteDePrueba("Zona Mismo Estado");

            IniciarSesion("operador@test.com", "Operador1@");
            Driver.Navigate().GoToUrl($"{BaseUrl}/Operador/Reportes");

            var fila = Driver.FindElement(By.XPath("//tr[td[contains(text(),'Zona Mismo Estado')]]"));
            var select = fila.FindElement(By.CssSelector("select[name='nuevoEstado']"));
            new OpenQA.Selenium.Support.UI.SelectElement(select).SelectByText("Reportado"); // mismo estado inicial

            fila.FindElement(By.CssSelector("button[type='submit']")).Click();

            Wait.Until(d => d.PageSource.Contains("Zona Mismo Estado"));

            Assert.That(Driver.Url, Does.Contain("/Operador/Reportes")); // no crasheó, sigue en la página
        }
    }
}