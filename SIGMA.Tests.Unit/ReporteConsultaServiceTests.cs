using SIGMA_PROJECT.Models;
using SIGMA_PROJECT.Services;
using Xunit;

namespace SIGMA.Tests.Unit
{
    public class ReporteConsultaServiceTests
    {
        private readonly ReporteConsultaService _service = new();

        [Fact] // Camino feliz
        public void Filtrar_PorZonaExistente_DevuelveCoincidencias()
        {
            var reportes = new List<ReporteApagon>
            {
                new() { Zona = "Los Ríos" },
                new() { Zona = "Centro" }
            };

            var resultado = _service.Filtrar(reportes, "Los Ríos", null);

            Assert.Single(resultado);
        }

        [Fact] // Negativo
        public void Filtrar_PorZonaInexistente_DevuelveListaVacia()
        {
            var reportes = new List<ReporteApagon> { new() { Zona = "Centro" } };

            var resultado = _service.Filtrar(reportes, "ZonaQueNoExiste", null);

            Assert.Empty(resultado);
        }

        [Fact] // Límite
        public void Filtrar_SinReportesEnLaColeccion_DevuelveListaVaciaSinExcepcion()
        {
            var reportes = new List<ReporteApagon>();

            var resultado = _service.Filtrar(reportes, null, null);

            Assert.Empty(resultado);
        }
    }
}