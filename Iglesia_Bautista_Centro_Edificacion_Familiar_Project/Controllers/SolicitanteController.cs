using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("PermitirTodo")]
    [ApiController]
    public class SolicitanteController : ControllerBase
    {

        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public SolicitanteController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;
        }

        // Método privado para guardar entidades
        private IActionResult GuardarEntidad<T>(T objeto, DbSet<T> dbSet, Func<T, bool> existeFunc, string mensajeExistente) where T : class
        {
            try
            {
                if (existeFunc(objeto))
                {
                    return StatusCode(StatusCodes.Status409Conflict, new { mensaje = mensajeExistente });
                }

                dbSet.Add(objeto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedEntity = objeto, mensaje = "La entidad se ha guardado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al guardar la entidad: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var solicitantes = _dbcontext.Solicitantes.Select(r => new
                {
                    r.IdSolicitante,
                    r.Nombre,
                    r.Apellido,
                    r.Cedula,
                    r.Dirrecion,
                    r.Telefono,
                    r.Correo,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "La petición realizada fue exitosa.", response = solicitantes });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al obtener la lista de solicitantes: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("BuscarSolicitante")]
        public IActionResult BuscarSolicitante(string? cedula, string? correo, string? nombreCompleto)
        {
            try
            {
                var query = _dbcontext.Solicitantes.AsQueryable();

                if (!string.IsNullOrEmpty(cedula))
                {
                    query = query.Where(s => s.Cedula.Contains(cedula));
                }

                if (!string.IsNullOrEmpty(correo))
                {
                    query = query.Where(s => s.Correo.Contains(correo));
                }

                if (!string.IsNullOrEmpty(nombreCompleto))
                {
                    query = query.Where(s => (s.Nombre + " " + s.Apellido).Contains(nombreCompleto));
                }

                var solicitantes = query.Select(r => new
                {
                    r.IdSolicitante,
                    r.Nombre,
                    r.Apellido,
                    r.Cedula,
                    r.Dirrecion,
                    r.Telefono,
                    r.Correo
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito.", response = solicitantes });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al realizar la búsqueda: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdSolicitante:int}")]
        public IActionResult Obtener(int IdSolicitante)
        {
            var solicitante = _dbcontext.Solicitantes.Find(IdSolicitante);

            if (solicitante == null)
            {
                return BadRequest("Lo siento, el solicitante no existe.");
            }

            try
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente.", response = solicitante });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al obtener el solicitante: " + ex.Message });
            }
        }
        [HttpPost]
        [Route("GuardarSolicitante")]
        public IActionResult GuardarSolicitante([FromBody] Solicitante objeto)
        {
            // Verifica si hay un responsable existente con la misma cédula que el solicitante
            if (_dbcontext.Responsables.Any(r => r.Cedula == objeto.Cedula))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El solicitante no puede tener la misma cédula que un responsable." });
            }

            return GuardarEntidad(objeto, _dbcontext.Solicitantes,
                s => _dbcontext.Solicitantes.Any(r => r.Cedula == s.Cedula),
                "Ya existe un solicitante con esa cédula.");
        }

        [HttpPost]
        [Route("GuardarResponsable")]
        public IActionResult GuardarResponsable([FromBody] Responsable objeto)
        {
            // Verifica si hay un solicitante existente con la misma cédula que el responsable
            if (_dbcontext.Solicitantes.Any(s => s.Cedula == objeto.Cedula))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El responsable no puede tener la misma cédula que un solicitante." });
            }

            return GuardarEntidad(objeto, _dbcontext.Responsables,
                r => _dbcontext.Responsables.Any(r => r.Cedula == objeto.Cedula),
                "Ya existe un responsable con esta cédula.");
        }


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Solicitante objeto)
        {
            var solicitanteExistente = _dbcontext.Solicitantes.Find(objeto.IdSolicitante);

            if (solicitanteExistente == null)
            {
                return BadRequest("Lo siento, su usuario no ha sido encontrado.");
            }

            // Verifica si hay un responsable existente con la misma cédula que el solicitante
            if (_dbcontext.Responsables.Any(r => r.Cedula == objeto.Cedula))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El solicitante no puede tener la misma cédula que un responsable." });
            }

            try
            {
                var existeCedula = _dbcontext.Solicitantes
                    .Any(s => s.Cedula == objeto.Cedula && s.IdSolicitante != objeto.IdSolicitante);

                if (existeCedula)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new { mensaje = "Ya existe un solicitante con esa cédula." });
                }

                solicitanteExistente.Nombre = objeto.Nombre;
                solicitanteExistente.Apellido = objeto.Apellido;
                solicitanteExistente.Cedula = objeto.Cedula;
                solicitanteExistente.Dirrecion = objeto.Dirrecion;
                solicitanteExistente.Telefono = objeto.Telefono;
                solicitanteExistente.Correo = objeto.Correo;

                _dbcontext.Solicitantes.Update(solicitanteExistente);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su solicitante se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al actualizar el solicitante: " + ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{IdSolicitante:int}")]
        public IActionResult Eliminar(int IdSolicitante)
        {
            var solicitante = _dbcontext.Solicitantes.Find(IdSolicitante);

            if (solicitante == null)
            {
                return BadRequest("Solicitante no encontrado.");
            }

            try
            {
                _dbcontext.Solicitantes.Remove(solicitante);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su solicitante se eliminó exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar el solicitante: " + ex.Message });
            }
        }
    }
}