using OpenQA.Selenium;
using NUnit.Framework;

namespace SIGMA.Tests.E2E
{
    public class CrearReporteTests : BaseTest
    {
        private void SetearFechaHora(string elementId, string valor)
        {
            var elemento = Driver.FindElement(By.Id(elementId));
            ((IJavaScriptExecutor)Driver).ExecuteScript($"arguments[0].value = '{valor}';", elemento);
        }

        [Test] // Camino feliz
        public void CrearReporte_ConDatosValidos_RedirigeAMisReportes()
        {
            IniciarSesion("ciudadano@test.com", "Ciudadano1@");

            Driver.Navigate().GoToUrl($"{BaseUrl}/Reportes/Crear");

            Driver.FindElement(By.Id("Zona")).SendKeys("Los Ríos");
            SetearFechaHora("FechaHora", "2026-08-15T10:00");
            Driver.FindElement(By.Id("DuracionMinutos")).SendKeys("45");
            Driver.FindElement(By.Id("Causa")).SendKeys("Sobrecarga eléctrica");

            Driver.FindElement(By.CssSelector("form[action='/Reportes/Crear'] button[type='submit']")).Click();

            Wait.Until(d => d.Url.Contains("MisReportes"));

            Assert.That(Driver.Url, Does.Contain("MisReportes"));
            Assert.That(Driver.PageSource, Does.Contain("Los Ríos"));
        }

        [Test] // Negativo — sin zona (campo obligatorio)
        public void CrearReporte_SinZona_MuestraErrorDeValidacion()
        {
            IniciarSesion("ciudadano@test.com", "Ciudadano1@");

            Driver.Navigate().GoToUrl($"{BaseUrl}/Reportes/Crear");

            // Zona queda vacía a propósito
            SetearFechaHora("FechaHora", "2026-08-15T10:00");
            Driver.FindElement(By.Id("DuracionMinutos")).SendKeys("20");

            Driver.FindElement(By.CssSelector("form[action='/Reportes/Crear'] button[type='submit']")).Click();

            Wait.Until(d => d.PageSource.Contains("required") || d.Url.Contains("/Reportes/Crear"));

            Assert.That(Driver.Url, Does.Contain("/Reportes/Crear")); // no navegó, se quedó en el formulario
            Assert.That(Driver.PageSource, Does.Contain("required"));
        }

        [Test] // Límite — duración = 0 (borde inferior permitido)
        public void CrearReporte_ConDuracionCero_SeGuardaCorrectamente()
        {
            IniciarSesion("ciudadano@test.com", "Ciudadano1@");

            Driver.Navigate().GoToUrl($"{BaseUrl}/Reportes/Crear");

            Driver.FindElement(By.Id("Zona")).SendKeys("Zona Límite");
            SetearFechaHora("FechaHora", "2026-08-15T10:00");
            Driver.FindElement(By.Id("DuracionMinutos")).SendKeys("0");

            Driver.FindElement(By.CssSelector("form[action='/Reportes/Crear'] button[type='submit']")).Click();

            Wait.Until(d => d.Url.Contains("MisReportes"));

            Assert.That(Driver.PageSource, Does.Contain("Zona Límite"));
        }
    }
}