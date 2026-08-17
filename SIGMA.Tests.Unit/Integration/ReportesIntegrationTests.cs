using Microsoft.EntityFrameworkCore;
using SIGMA_PROJECT.Data;
using SIGMA_PROJECT.Models;
using SIGMA_PROJECT.Services;
using Xunit;

namespace SIGMA.Tests.Unit.Integration
{
    public class ReportesIntegrationTests
    {
        private ApplicationDbContext CrearContextoEnMemoria()
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // BD nueva y aislada por cada prueba
                .Options;

            return new ApplicationDbContext(opciones);
        }

        [Fact] // Camino feliz
        public async Task CrearReporte_ConDatosValidos_SePersisteEnBaseDeDatos()
        {
            using var context = CrearContextoEnMemoria();

            var reporte = new ReporteApagon
            {
                Zona = "Los Ríos",
                FechaHora = DateTime.Now,
                DuracionMinutos = 30,
                Causa = "Sobrecarga",
                CiudadanoId = "usuario-test-123"
            };

            context.Reportes.Add(reporte);
            await context.SaveChangesAsync();

            var reporteGuardado = await context.Reportes.FirstOrDefaultAsync(r => r.Zona == "Los Ríos");

            Assert.NotNull(reporteGuardado);
            Assert.Equal(EstadoReporte.Reportado, reporteGuardado.Estado);
        }

        [Fact] // Camino feliz — consulta filtrada por ciudadano (HU-04)
        public async Task ConsultarMisReportes_DevuelveSoloReportesDelCiudadanoAutenticado()
        {
            using var context = CrearContextoEnMemoria();

            context.Reportes.AddRange(
                new ReporteApagon { Zona = "A", CiudadanoId = "user-1", FechaHora = DateTime.Now },
                new ReporteApagon { Zona = "B", CiudadanoId = "user-2", FechaHora = DateTime.Now },
                new ReporteApagon { Zona = "C", CiudadanoId = "user-1", FechaHora = DateTime.Now }
            );
            await context.SaveChangesAsync();

            var reportesDeUser1 = await context.Reportes
                .Where(r => r.CiudadanoId == "user-1")
                .ToListAsync();

            Assert.Equal(2, reportesDeUser1.Count);
        }

        [Fact] // Negativo — cambio de estado persiste correctamente con auditoría (HU-06 + RNF-02)
        public async Task CambiarEstadoReporte_GeneraRegistroDeAuditoria()
        {
            using var context = CrearContextoEnMemoria();

            var reporte = new ReporteApagon { Zona = "Centro", FechaHora = DateTime.Now, CiudadanoId = "user-1" };
            context.Reportes.Add(reporte);
            await context.SaveChangesAsync();

            var estadoAnterior = reporte.Estado;
            reporte.Estado = EstadoReporte.EnInvestigacion;

            context.Auditorias.Add(new RegistroAuditoria
            {
                ReporteId = reporte.Id,
                OperadorId = "operador-1",
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = EstadoReporte.EnInvestigacion
            });
            await context.SaveChangesAsync();

            var auditoria = await context.Auditorias.FirstOrDefaultAsync(a => a.ReporteId == reporte.Id);

            Assert.NotNull(auditoria);
            Assert.Equal(EstadoReporte.Reportado, auditoria.EstadoAnterior);
            Assert.Equal(EstadoReporte.EnInvestigacion, auditoria.EstadoNuevo);
        }

        [Fact] // Límite — reporte sin duración registrada (campo opcional)
        public async Task CrearReporte_SinDuracion_SePersisteConValorNulo()
        {
            using var context = CrearContextoEnMemoria();

            var reporte = new ReporteApagon
            {
                Zona = "Norte",
                FechaHora = DateTime.Now,
                DuracionMinutos = null,
                CiudadanoId = "user-1"
            };

            context.Reportes.Add(reporte);
            await context.SaveChangesAsync();

            var guardado = await context.Reportes.FirstAsync(r => r.Zona == "Norte");
            Assert.Null(guardado.DuracionMinutos);
        }

        [Fact] // Read — Negativo: ciudadano sin reportes
        public async Task ConsultarMisReportes_CiudadanoSinReportes_DevuelveListaVacia()
        {
            using var context = CrearContextoEnMemoria();
            context.Reportes.Add(new ReporteApagon { Zona = "A", CiudadanoId = "otro-user", FechaHora = DateTime.Now });
            await context.SaveChangesAsync();

            var reportes = await context.Reportes
                .Where(r => r.CiudadanoId == "user-sin-reportes")
                .ToListAsync();

            Assert.Empty(reportes);
        }

        [Fact] // Update — Negativo: transición inválida no debe aplicarse (validada antes de persistir)
        public async Task CambiarEstado_TransicionInvalida_NoSePersisteElCambio()
        {
            using var context = CrearContextoEnMemoria();
            var estadoService = new ReporteEstadoService();

            var reporte = new ReporteApagon { Zona = "Sur", CiudadanoId = "user-1", Estado = EstadoReporte.Resuelto };
            context.Reportes.Add(reporte);
            await context.SaveChangesAsync();

            var transicionValida = estadoService.EsTransicionValida(reporte.Estado, EstadoReporte.Reportado);
            if (transicionValida)
            {
                reporte.Estado = EstadoReporte.Reportado;
                await context.SaveChangesAsync();
            }

            var reporteFinal = await context.Reportes.FirstAsync(r => r.Id == reporte.Id);
            Assert.Equal(EstadoReporte.Resuelto, reporteFinal.Estado); // no cambió, la transición fue rechazada
        }

        [Fact] // Update — Límite: cambiar al mismo estado no genera inconsistencia
        public async Task CambiarEstado_AlMismoEstado_SePersisteSinError()
        {
            using var context = CrearContextoEnMemoria();
            var reporte = new ReporteApagon { Zona = "Este", CiudadanoId = "user-1", Estado = EstadoReporte.EnInvestigacion };
            context.Reportes.Add(reporte);
            await context.SaveChangesAsync();

            reporte.Estado = EstadoReporte.EnInvestigacion; // mismo estado
            await context.SaveChangesAsync();

            var reporteFinal = await context.Reportes.FirstAsync(r => r.Id == reporte.Id);
            Assert.Equal(EstadoReporte.EnInvestigacion, reporteFinal.Estado);
        }
    }
}