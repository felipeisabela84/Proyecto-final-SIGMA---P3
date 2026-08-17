using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class ReporteApagon
{
    public int Id { get; set; }
    public string Zona { get; set; }
    public DateTime FechaHora { get; set; }
    public int? DuracionMinutos { get; set; }
    public string? Causa { get; set; }
    public EstadoReporte Estado { get; set; } = EstadoReporte.Reportado;

    [ValidateNever]
    public string CiudadanoId { get; set; }

    [ValidateNever]
    public IdentityUser Ciudadano { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    
}
public enum EstadoReporte
{
    Reportado,
    EnInvestigacion,
    Resuelto
}