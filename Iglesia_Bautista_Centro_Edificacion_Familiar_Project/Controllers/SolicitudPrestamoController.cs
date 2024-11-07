using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO;
using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("PermitirTodo")]
    [ApiController]
    public class SolicitudPrestamoController : ControllerBase
    {


        private readonly dbIglesia_Bautista_Centro_FamiliarContext _context;

        public SolicitudPrestamoController(dbIglesia_Bautista_Centro_FamiliarContext context)
        {
            _context = context;
        }

        [HttpPost("Guardar")]
        public IActionResult GuardarSolicitudPrestamo([FromBody] SolicitudPrestamo solicitud)
        {
            if (solicitud == null)
            {
                return BadRequest("Solicitud de préstamo no puede ser nula.");
            }

            // Obtener el inventario correspondiente
            var inventario = _context.Inventarios
                .FirstOrDefault(i => i.IdInventario == solicitud.IdInventario);

            if (inventario == null)
            {
                return NotFound("Inventario no encontrado.");
            }

            // Validar que la cantidad no exceda el stock máximo
            if (solicitud.Cantidad > inventario.StockMaximo)
            {
                return BadRequest("La cantidad solicitada excede el stock máximo disponible.");
            }

            // Si todo es válido, agrega la solicitud
            _context.SolicitudPrestamos.Add(solicitud);

            // Restar la cantidad solicitada del StockMaximo
            inventario.StockMaximo -= solicitud.Cantidad;

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return Ok("Solicitud de préstamo guardada con éxito.");
        }



        [HttpDelete("Eliminar/{IdSolicituPrestamo}")]
        public IActionResult EliminarSolicitudPrestamo(int IdSolicituPrestamo)
        {
            // Buscar la solicitud de préstamo por ID
            var solicitud = _context.SolicitudPrestamos.FirstOrDefault(s => s.IdSolicitudPrestamo == IdSolicituPrestamo);

            if (solicitud == null)
            {
                return NotFound("Solicitud de préstamo no encontrada.");
            }

            // Obtener el inventario correspondiente
            var inventario = _context.Inventarios
                .FirstOrDefault(i => i.IdInventario == solicitud.IdInventario);

            if (inventario == null)
            {
                return NotFound("Inventario no encontrado.");
            }

            // Restaurar la cantidad al StockMaximo
            inventario.StockMaximo += solicitud.Cantidad;

            // Eliminar la solicitud de préstamo
            _context.SolicitudPrestamos.Remove(solicitud);

            // Guardar cambios en la base de datos
            _context.SaveChanges();

            return Ok("Solicitud de préstamo eliminada con éxito.");
        }




        [HttpGet("Lista")]
        public IActionResult ObtenerSolicitudesPrestamo()
        {
            var solicitudes = _context.SolicitudPrestamos
                .Select(s => new SolicitudPrestamoDTO
                {
                    IdSolicituPrestamo = s.IdSolicitudPrestamo,
                    IdInventario = s.IdInventario,
                    Cantidad = s.Cantidad,
                    FechaOperaciones = s.FechaOperaciones,
                    Lugar = s.Lugar,
                    FechaEntrega = s.FechaEntrega,
                    IdResponsable = s.IdResponsable,

                    // Si estás obteniendo el nombre del responsable, hazlo aquí
                    NombreResponsable = s.IdResponsableNavigation != null
                        ? s.IdResponsableNavigation.Nombre // Asumiendo que `Nombre` es la propiedad correcta
                        : null,
                })
                .ToList();

            return Ok(solicitudes);
        }


    }


}

