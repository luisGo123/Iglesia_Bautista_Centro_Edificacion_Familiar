using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AprovadoEntregaController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public AprovadoEntregaController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }


        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var AprovadoEntrega = _dbcontext.AprovadoEntregas.Select(r => new
                {
                    r.IdAprovadoEntrega,
                    r.IdSolicitante,
                    r.IdSolicitudPrestamo,
                    r.FechaAprovacion,
                    r.Cantidad,
                    r.Observacion,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = AprovadoEntrega });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }



        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] AprovadoEntrega objeto)
        {
            try
            {
                // Obtener la solicitud de préstamo correspondiente
                var solicitudPrestamo = _dbcontext.SolicitudPrestamos
                                                  .FirstOrDefault(s => s.IdSolicitudPrestamo == objeto.IdSolicitudPrestamo);

                // Verificar si existe la solicitud de préstamo
                if (solicitudPrestamo == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "La solicitud de préstamo no existe." });
                }

                // Verificar si la cantidad excede la cantidad de la solicitud de préstamo
                if (objeto.Cantidad > solicitudPrestamo.Cantidad)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "La cantidad aprobada no puede ser mayor que la cantidad solicitada." });
                }

                // Crear el objeto AprovadoEntrega
                var creacionAprovadoEntrega = new AprovadoEntrega
                {
                    IdSolicitante = objeto.IdSolicitante,
                    IdSolicitudPrestamo = objeto.IdSolicitudPrestamo,
                    FechaAprovacion = DateTime.Now, // Suponiendo que la fecha de aprobación es ahora
                    Cantidad = objeto.Cantidad,
                    Observacion = objeto.Observacion
                };

                // Guardar en la base de datos
                _dbcontext.AprovadoEntregas.Add(creacionAprovadoEntrega);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionAprovadoEntrega, mensaje = "Se ha guardado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{IdAprovadoEntrega:int}")]
        public IActionResult Eliminar(int IdAprovadoEntrega)
        {

            AprovadoEntrega AprovadoEntregas = _dbcontext.AprovadoEntregas.Find(IdAprovadoEntrega);

            if (AprovadoEntregas == null)
            {
                return BadRequest(" no encontrado");

            }

            try
            {

                _dbcontext.AprovadoEntregas.Remove(AprovadoEntregas);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " Se Elimino Exitosamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

    }
}

