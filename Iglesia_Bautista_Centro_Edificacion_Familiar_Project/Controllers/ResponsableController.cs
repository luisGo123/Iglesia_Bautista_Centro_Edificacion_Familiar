using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsableController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public ResponsableController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var Responsable = _dbcontext.Responsables.Select(r => new
                {
                    r.IdResponsable,
                    r.Nombre,
                    r.Cedula,
                    r.IdCargo,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = Responsable });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("Buscar")]
        public IActionResult Buscar(string nombre = null, string cedula = null)
        {
            if (string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(cedula))
            {
                return BadRequest("Se debe proporcionar al menos un parámetro: nombre o cedula.");
            }

            var responsables = _dbcontext.Responsables.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                responsables = responsables.Where(r => r.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(cedula))
            {
                responsables = responsables.Where(r => r.Cedula.Contains(cedula)); // Permitir coincidencias parciales
            }

            var listaResponsables = responsables.ToList();

            if (!listaResponsables.Any())
            {
                return NotFound("No se encontraron responsables que coincidan con los criterios de búsqueda.");
            }

            try
            {
                var respuesta = listaResponsables.Select(r => new
                {
                    r.IdResponsable,
                    r.Nombre,
                    r.Cedula,
                    r.IdCargo
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada exitosamente", response = respuesta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }



        [HttpGet]
        [Route("Obtener/{IdResponsable:int}")]
        public IActionResult Obtener(int IdResponsable)
        {
            var responsable = _dbcontext.Responsables.Find(IdResponsable);

            if (responsable == null)
            {
                return BadRequest("Lo siento, el responsable no existe.");
            }

            try
            {
                var respuesta = new
                {
                    responsable.IdResponsable,
                    responsable.Nombre,
                    responsable.Cedula,
                    responsable.IdCargo
                };

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = respuesta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }






        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Responsable objeto)
        {
            try
            {
                // Verificar si ya existe un usuario con la misma cédula
                var existeUsuario = _dbcontext.Responsables
                    .Any(u => u.Cedula == objeto.Cedula);

                if (existeUsuario)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un Responsable con esta cédula." }); 
                }

                var creacionResponsable = new Responsable
                {
                    Nombre = objeto.Nombre,
                    Cedula = objeto.Cedula,
                    IdCargo = objeto.IdCargo,
                   
                };

                _dbcontext.Responsables.Add(creacionResponsable);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionResponsable, mensaje = "Su Responsable se ha creado y guardado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }





        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Responsable objeto)
        {
            var responsableExistente = _dbcontext.Responsables.Find(objeto.IdResponsable);

            if (responsableExistente == null)
            {
                return BadRequest("Lo siento, su petición no ha sido encontrada.");
            }

            try
            {
                // Verificar si ya existe un solicitante con la misma cédula
                if (_dbcontext.Solicitantes.Any(s => s.Cedula == objeto.Cedula))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El responsable no puede tener la misma cédula que un solicitante." });
                }

                // Verificar si ya existe otro responsable con la misma cédula
                var existeResponsable = _dbcontext.Responsables
                    .Any(r => r.Cedula == objeto.Cedula && r.IdResponsable != objeto.IdResponsable);

                if (existeResponsable)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un responsable con esta cédula." }); // 409 Conflict
                }

                responsableExistente.Nombre = objeto.Nombre;
                responsableExistente.Cedula = objeto.Cedula;
                responsableExistente.IdCargo = objeto.IdCargo;

                _dbcontext.Responsables.Update(responsableExistente);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{IdResponsable:int}")]
        public IActionResult Eliminar(int IdResponsable)
        {

            Responsable Responsables = _dbcontext.Responsables.Find(IdResponsable);

            if (Responsables == null)
            {
                return BadRequest(" no encontrado");

            }

            try
            {

                _dbcontext.Responsables.Remove(Responsables);
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

    
