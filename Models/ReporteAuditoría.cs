
public class RegistroAuditoria
{
    public int Id { get; set; }
    public int ReporteId { get; set; }
    public string OperadorId { get; set; }
    public EstadoReporte EstadoAnterior { get; set; }
    public EstadoReporte EstadoNuevo { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
}