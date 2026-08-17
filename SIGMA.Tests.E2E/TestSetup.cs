using NUnit.Framework;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace SIGMA.Tests.E2E
{
    [SetUpFixture]
    public class TestSetup
    {

        public static ExtentReports Extent;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            string carpetaReporte = Path.Combine(TestContext.CurrentContext.TestDirectory, "ReporteHTML");
            if (Directory.Exists(carpetaReporte))
            {
                Directory.Delete(carpetaReporte, recursive: true);
            }
            string ruta = Path.Combine(TestContext.CurrentContext.TestDirectory, "ReporteHTML", "ReporteSIGMA.html");
            Console.WriteLine("REPORTE SE GUARDARÁ EN: " + ruta); // diagnóstico temporal

            var htmlReporter = new ExtentSparkReporter(ruta);
            Extent = new ExtentReports();
            Extent.AttachReporter(htmlReporter);
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            Extent.Flush();
        }
    }
}