namespace SIGMA_PROJECT.Services
{
    public class ReporteValidacionService
    {
        public (bool EsValido, string? Error) Validar(ReporteApagon reporte)
        {
            if (string.IsNullOrWhiteSpace(reporte.Zona))
                return (false, "La zona es obligatoria.");

            if (reporte.DuracionMinutos.HasValue && reporte.DuracionMinutos < 0)
                return (false, "La duración no puede ser negativa.");

            return (true, null);
        }
    }
}