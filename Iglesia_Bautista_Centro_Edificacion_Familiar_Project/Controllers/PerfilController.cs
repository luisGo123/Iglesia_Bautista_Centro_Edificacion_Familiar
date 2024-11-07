using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("PermitirTodo")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public PerfilController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var perfiles = _dbcontext.Perfils.Select(r => new
                {
                    r.IdPerfil,
                    r.Nombre,
                    r.IdPermiso,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = perfiles });
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
                var query = _dbcontext.Perfils.AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(p => p.Nombre.Contains(nombre));
                }

                var perfiles = query.Select(r => new
                {
                    r.IdPerfil,
                    r.Nombre,
                    r.IdPermiso
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = perfiles });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("Obtener/{IdPerfil:int}")]
        public IActionResult Obtener(int IdPerfil)
        {
            Perfil perfils = _dbcontext.Perfils.Find(IdPerfil);

            if (perfils == null)
            {
                return BadRequest(" lo siento El Rol no existe");

            }

            try
            {

                var Perfil = _dbcontext.Perfils.Select(r => new
                {
                    r.IdPerfil,
                    r.Nombre,
                    r.IdPermiso,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = perfils });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Perfil objeto)
        {
            try
            {
                var creacionrol = new Perfil { Nombre = objeto.Nombre, IdPermiso = objeto.IdPermiso };


                _dbcontext.Perfils.Add(creacionrol);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionrol, mensaje = "Su perfil se ha creado y Guardado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Perfil objeto)
        {
            Perfil Perfils = _dbcontext.Perfils.Find(objeto.IdPerfil);

            if (Perfils == null)
            {
                return BadRequest(" lo siento su perfil no ha sido encomtrado ");
            }

            try
            {
                Perfils.Nombre = objeto.Nombre;
                Perfils.IdPermiso = objeto.IdPermiso;


                _dbcontext.Perfils.Update(Perfils);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su perfil Se Actualizo Correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdRol:int}")]
        public IActionResult Eliminar(int IdRol)
        {

            Perfil perfil = _dbcontext.Perfils.Find(IdRol);

            if (perfil == null)
            {
                return BadRequest("perfil no encontrado");

            }

            try
            {

                _dbcontext.Perfils.Remove(perfil);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su perfil Se Elimino Exitosamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

    }
}