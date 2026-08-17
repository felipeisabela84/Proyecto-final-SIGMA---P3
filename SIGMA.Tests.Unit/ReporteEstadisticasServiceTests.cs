using SIGMA_PROJECT.Models;
using SIGMA_PROJECT.Services;
using Xunit;

namespace SIGMA.Tests.Unit
{
    public class ReporteEstadisticasServiceTests
    {
        private readonly ReporteEstadisticasService _service = new();

        [Fact] // Camino feliz
        public void ReportesPorZona_ConVariosReportes_AgrupaCorrectamente()
        {
            var reportes = new List<ReporteApagon>
            {
                new() { Zona = "Los Ríos" },
                new() { Zona = "Los Ríos" },
                new() { Zona = "Centro" }
            };

            var resultado = _service.ReportesPorZona(reportes);

            Assert.Equal(2, resultado["Los Ríos"]);
            Assert.Equal(1, resultado["Centro"]);
        }

        [Fact] // Límite — sin reportes con duración, no debe dividir por cero
        public void DuracionPromedio_SinReportesConDuracion_DevuelveCero()
        {
            var reportes = new List<ReporteApagon>
            {
                new() { Zona = "A", DuracionMinutos = null },
                new() { Zona = "B", DuracionMinutos = null }
            };

            var resultado = _service.DuracionPromedio(reportes);

            Assert.Equal(0, resultado);
        }

        [Fact] // Negativo — colección vacía no debe lanzar excepción
        public void CausasMasFrecuentes_SinReportes_DevuelveDiccionarioVacio()
        {
            var reportes = new List<ReporteApagon>();

            var resultado = _service.CausasMasFrecuentes(reportes);

            Assert.Empty(resultado);
        }
    }
}