using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO;
using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionController : ControllerBase
    {
        private readonly dbIglesia_Bautista_Centro_FamiliarContext _context;

        public DevolucionController(dbIglesia_Bautista_Centro_FamiliarContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearDevolucion([FromBody] DeVolucion devolucion)
        {
            if (devolucion == null)
            {
                return BadRequest("Datos de devolución incompletos.");
            }

            // Validar que la cantidad y el idInventario no sean nulos
            if (!devolucion.Cantidad.HasValue || !devolucion.IdInventario.HasValue)
            {
                return BadRequest("La cantidad y el idInventario son requeridos.");
            }

            // Buscar el inventario asociado
            var inventario = await _context.Inventarios.FindAsync(devolucion.IdInventario);
            if (inventario == null)
            {
                return NotFound("Inventario no encontrado.");
            }

            // Sumar la cantidad de devolución al StockMaximo
            inventario.StockMaximo += devolucion.Cantidad.Value;

            // Guardar la devolución
            _context.DeVolucions.Add(devolucion);
            await _context.SaveChangesAsync();

            return Ok(devolucion);
        }



        [HttpGet("ListaInactivos")]
        public async Task<ActionResult<IEnumerable<DevolucionDTO>>> ListaInactivos()
        {

            var inventariosInactivos = await _context.DeVolucions
                .Include(i => i.IdInventarioNavigation)
                .Select(i => new DevolucionDTO
                {
                    IdDeVolucion = i.IdDeVolucion,
                    FechaDevolucion = i.FechaDevolucion,
                    Observacion = i.Observacion,
                    Cantidad = i.Cantidad,


                    IdInventario = i.IdInventario,
                    
                    FechaInventario = i.IdInventarioNavigation.FechaInventario,
                    StockMinimo = i.IdInventarioNavigation.StockMinimo,
                    StockMaximo = i.IdInventarioNavigation.StockMaximo,
                  
             
                })
            .ToListAsync();

            return Ok(inventariosInactivos);
        }

        [HttpDelete("{idDeVolucion}")]
        public async Task<IActionResult> EliminarDevolucion(int idDeVolucion)
        {
            // Buscar la devolución por ID
            var devolucion = await _context.DeVolucions.FindAsync(idDeVolucion);
            if (devolucion == null)
            {
                return NotFound("Devolución no encontrada.");
            }

            // Eliminar la devolución
            _context.DeVolucions.Remove(devolucion);
            await _context.SaveChangesAsync();

            return Ok("Devolución eliminada con éxito.");
        }

    }
}
