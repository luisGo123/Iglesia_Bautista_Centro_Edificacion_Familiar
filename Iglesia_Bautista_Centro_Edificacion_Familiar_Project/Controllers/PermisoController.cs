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
    public class PermisoController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public PermisoController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var Permisos = _dbcontext.Permisos.Select(r => new
                {
                    r.IdPermiso,
                    r.Nombre,


                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = Permisos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }




        [HttpGet]
        [Route("Buscar")]
        public IActionResult Buscar(string? nombre)
        {
            try
            {
                var query = _dbcontext.Permisos.AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(p => p.Nombre.Contains(nombre));
                }

                var permisos = query.Select(r => new
                {
                    r.IdPermiso,
                    r.Nombre
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = permisos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("Obtener/{IdPermiso:int}")]
        public IActionResult Obtener(int IdPermiso)
        {
            Permiso Permisos = _dbcontext.Permisos.Find(IdPermiso);

            if (Permisos == null)
            {
                return BadRequest(" lo siento, no existe");

            }

            try
            {

                var Permiso = _dbcontext.Permisos.Select(r => new
                {
                    r.IdPermiso,
                    r.Nombre,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = Permisos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Permiso objeto)
        {
            try
            {
                var creacionPermiso = new Permiso { Nombre = objeto.Nombre, };


                _dbcontext.Permisos.Add(creacionPermiso);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionPermiso, mensaje = " se ha creado y Guardado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Permiso objeto)
        {
            Permiso Permisos = _dbcontext.Permisos.Find(objeto.IdPermiso);

            if (Permisos == null)
            {
                return BadRequest(" lo siento, no ha sido encomtrado ");
            }

            try
            {
                Permisos.Nombre = objeto.Nombre;



                _dbcontext.Permisos.Update(Permisos);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " Se Actualizo Correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdPermiso:int}")]
        public IActionResult Eliminar(int IdPermiso)
        {

            Permiso Permisos = _dbcontext.Permisos.Find(IdPermiso);

            if (Permisos == null)
            {
                return BadRequest("perfil no encontrado");

            }

            try
            {

                _dbcontext.Permisos.Remove(Permisos);
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