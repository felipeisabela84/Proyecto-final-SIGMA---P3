using SIGMA_PROJECT.Models;
using SIGMA_PROJECT.Services;
using Xunit;

namespace SIGMA.Tests.Unit
{
    public class ReporteEstadoServiceTests
    {
        private readonly ReporteEstadoService _service = new();

        [Fact] // Camino feliz
        public void Transicion_ReportadoAEnInvestigacion_EsValida()
        {
            var resultado = _service.EsTransicionValida(EstadoReporte.Reportado, EstadoReporte.EnInvestigacion);
            Assert.True(resultado);
        }

        [Fact] // Camino feliz
        public void Transicion_EnInvestigacionAResuelto_EsValida()
        {
            var resultado = _service.EsTransicionValida(EstadoReporte.EnInvestigacion, EstadoReporte.Resuelto);
            Assert.True(resultado);
        }

        [Fact] // Negativo
        public void Transicion_ResueltoAReportado_NoEsValida()
        {
            var resultado = _service.EsTransicionValida(EstadoReporte.Resuelto, EstadoReporte.Reportado);
            Assert.False(resultado);
        }

        [Fact] // Negativo — salto de estado
        public void Transicion_ReportadoAResuelto_NoEsValida()
        {
            var resultado = _service.EsTransicionValida(EstadoReporte.Reportado, EstadoReporte.Resuelto);
            Assert.False(resultado);
        }

        [Fact] // Límite — mismo estado (sin cambio)
        public void Transicion_MismoEstado_EsValida()
        {
            var resultado = _service.EsTransicionValida(EstadoReporte.EnInvestigacion, EstadoReporte.EnInvestigacion);
            Assert.True(resultado);
        }
    }
}