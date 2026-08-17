namespace SIGMA_PROJECT.Services
{
    public class ReporteConsultaService
    {
        public List<ReporteApagon> Filtrar(IEnumerable<ReporteApagon> reportes, string? zona, EstadoReporte? estado)
        {
            var query = reportes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(zona))
                query = query.Where(r => r.Zona.Contains(zona, StringComparison.OrdinalIgnoreCase));

            if (estado.HasValue)
                query = query.Where(r => r.Estado == estado.Value);

            return query.ToList();
        }
    }
}