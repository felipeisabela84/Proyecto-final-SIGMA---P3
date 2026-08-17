public class Notificacion
{
    public int Id { get; set; }
    public int ReporteId { get; set; }
    public ReporteApagon Reporte { get; set; }
    public string CiudadanoId { get; set; }
    public string Mensaje { get; set; }
    public bool Leida { get; set; } = false;
    public DateTime Fecha { get; set; } = DateTime.Now;
}