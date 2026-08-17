using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGMA_PROJECT.Data;
using SIGMA_PROJECT.Models;
using SIGMA_PROJECT.Services;

[Authorize(Roles = "Operador")]
public class OperadorController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ReporteEstadoService _estadoService;
    private readonly ReporteEstadisticasService _estadisticasService;
    public OperadorController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ReporteEstadoService estadoService, ReporteEstadisticasService estadisticasService)

    {
        _context = context;
        _userManager = userManager;
        _estadoService = estadoService;
        _estadisticasService = estadisticasService;
    }

    // GET: Operador/Reportes  (HU-05)
    public async Task<IActionResult> Reportes(string zona, EstadoReporte? estado)
    {
        var query = _context.Reportes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(zona))
            query = query.Where(r => r.Zona.Contains(zona));

        if (estado.HasValue)
            query = query.Where(r => r.Estado == estado.Value);

        var reportes = await query.OrderByDescending(r => r.FechaCreacion).ToListAsync();

        ViewBag.Zona = zona;
        ViewBag.Estado = estado;
        return View(reportes);
    }

    // POST: Operador/CambiarEstado  (HU-06 + auditoría RNF-02)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int reporteId, EstadoReporte nuevoEstado)
    {
        var reporte = await _context.Reportes.FindAsync(reporteId);
        if (reporte == null) return NotFound();

        var estadoAnterior = reporte.Estado;
        reporte.Estado = nuevoEstado;

        if (!_estadoService.EsTransicionValida(reporte.Estado, nuevoEstado))
        {
            TempData["Error"] = "Transición de estado no permitida.";
            return RedirectToAction(nameof(Reportes));
        }

        _context.Auditorias.Add(new RegistroAuditoria
        {
            ReporteId = reporte.Id,
            OperadorId = _userManager.GetUserId(User),
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = nuevoEstado,
            Fecha = DateTime.Now
        });

        _context.Notificaciones.Add(new Notificacion
        {
            ReporteId = reporte.Id,
            CiudadanoId = reporte.CiudadanoId,
            Mensaje = $"Tu reporte de apagón en {reporte.Zona} cambió a estado: {nuevoEstado}."
        });

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Reportes));


    }
    public async Task<IActionResult> Dashboard()
    {
        var reportes = await _context.Reportes.ToListAsync();

        ViewBag.PorZona = _estadisticasService.ReportesPorZona(reportes);
        ViewBag.DuracionPromedio = _estadisticasService.DuracionPromedio(reportes);
        ViewBag.CausasFrecuentes = _estadisticasService.CausasMasFrecuentes(reportes);

        return View();
    }
}