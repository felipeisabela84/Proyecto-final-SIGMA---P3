using SIGMA_PROJECT.Models;

namespace SIGMA_PROJECT.Services
{
    public class ReporteEstadisticasService
    {
        public Dictionary<string, int> ReportesPorZona(IEnumerable<ReporteApagon> reportes)
        {
            return reportes
                .GroupBy(r => r.Zona)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public double DuracionPromedio(IEnumerable<ReporteApagon> reportes)
        {
            var conDuracion = reportes.Where(r => r.DuracionMinutos.HasValue).ToList();
            if (!conDuracion.Any()) return 0;
            return conDuracion.Average(r => r.DuracionMinutos!.Value);
        }

        public Dictionary<string, int> CausasMasFrecuentes(IEnumerable<ReporteApagon> reportes)
        {
            return reportes
                .Where(r => !string.IsNullOrWhiteSpace(r.Causa))
                .GroupBy(r => r.Causa!)
                .OrderByDescending(g => g.Count())
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}