using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIGMA_PROJECT.Data;
using Microsoft.EntityFrameworkCore;
using SIGMA_PROJECT.Models;

[Authorize(Roles = "Ciudadano")]
public class ReportesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ReportesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Reportes/Crear
    public IActionResult Crear() => View();

    // POST: Reportes/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ReporteApagon reporte)
    {
        if (string.IsNullOrWhiteSpace(reporte.Zona))
            ModelState.AddModelError(nameof(reporte.Zona), "La zona es obligatoria.");

        if (!ModelState.IsValid)
            return View(reporte);

        reporte.CiudadanoId = _userManager.GetUserId(User);
        reporte.Estado = EstadoReporte.Reportado;
        reporte.FechaCreacion = DateTime.Now;

        _context.Reportes.Add(reporte);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(MisReportes));
    }

    // GET: Reportes/MisReportes  (HU-04)
    public async Task<IActionResult> MisReportes()
    {
        var userId = _userManager.GetUserId(User);
        var reportes = await _context.Reportes
            .Where(r => r.CiudadanoId == userId)
            .OrderByDescending(r => r.FechaCreacion)
            .ToListAsync();

        return View(reportes);
    }

    // GET: Reportes/Notificaciones
    public async Task<IActionResult> Notificaciones()
    {
        var userId = _userManager.GetUserId(User);
        var notificaciones = await _context.Notificaciones
            .Where(n => n.CiudadanoId == userId)
            .OrderByDescending(n => n.Fecha)
            .ToListAsync();

        return View(notificaciones);
    }
}