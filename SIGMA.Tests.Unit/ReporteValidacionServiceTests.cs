using SIGMA_PROJECT;
using SIGMA_PROJECT.Services;
using Xunit;

namespace SIGMA.Tests.Unit
{
    public class ReporteValidacionServiceTests
    {
        private readonly ReporteValidacionService _service = new();

        [Fact] // Camino feliz
        public void Validar_ConZonaYDuracionValidas_EsValido()
        {
            var reporte = new ReporteApagon { Zona = "Los Ríos", DuracionMinutos = 30 };
            var (esValido, error) = _service.Validar(reporte);

            Assert.True(esValido);
            Assert.Null(error);
        }

        [Fact] // Negativo
        public void Validar_SinZona_NoEsValido()
        {
            var reporte = new ReporteApagon { Zona = "", DuracionMinutos = 30 };
            var (esValido, error) = _service.Validar(reporte);

            Assert.False(esValido);
            Assert.Equal("La zona es obligatoria.", error);
        }

        [Fact] // Límite
        public void Validar_ConDuracionCero_EsValido()
        {
            var reporte = new ReporteApagon { Zona = "Centro", DuracionMinutos = 0 };
            var (esValido, _) = _service.Validar(reporte);

            Assert.True(esValido); // 0 es el borde inferior permitido, no negativo
        }
    }
}