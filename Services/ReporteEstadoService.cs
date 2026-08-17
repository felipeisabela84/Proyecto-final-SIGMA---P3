using SIGMA_PROJECT.Models;

namespace SIGMA_PROJECT.Services
{
        public class ReporteEstadoService
        {
            public bool EsTransicionValida(EstadoReporte actual, EstadoReporte nuevo)
            {
                return (actual, nuevo) switch
                {
                    (EstadoReporte.Reportado, EstadoReporte.EnInvestigacion) => true,
                    (EstadoReporte.EnInvestigacion, EstadoReporte.Resuelto) => true,
                    (var a, var b) when a == b => true, // permitir "sin cambio"
                    _ => false
                };
            }
        }
}
