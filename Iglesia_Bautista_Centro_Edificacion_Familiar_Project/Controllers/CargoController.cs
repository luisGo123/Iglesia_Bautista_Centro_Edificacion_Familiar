using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
 
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public CargoController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var Cargo = _dbcontext.Cargos.Select(r => new
                {
                    r.IdCargo,
                    r.Nombre,


                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = Cargo });
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
                var query = _dbcontext.Cargos.AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(c => c.Nombre.Contains(nombre));
                }

                var cargos = query.Select(r => new
                {
                    r.IdCargo,
                    r.Nombre
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = cargos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }



        [HttpGet]
        [Route("Obtener/{IdCargo:int}")]
        public IActionResult Obtener(int IdCargo)
        {
            Cargo Cargos = _dbcontext.Cargos.Find(IdCargo);

            if (Cargos == null)
            {
                return BadRequest(" lo siento  no existe");

            }

            try
            {

                var Cargo = _dbcontext.Cargos.Select(r => new
                {
                    r.IdCargo,
                    r.Nombre,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = Cargo });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Cargo objeto)
        {
            try
            {
                var creacionCargo = new Cargo { Nombre = objeto.Nombre, };


                _dbcontext.Cargos.Add(creacionCargo);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionCargo, mensaje = " se ha creado y Guardado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Cargo objeto)
        {
            Cargo Cargos = _dbcontext.Cargos.Find(objeto.IdCargo);

            if (Cargos == null)
            {
                return BadRequest(" lo siento, no ha sido encomtrado ");
            }

            try
            {
                Cargos.Nombre = objeto.Nombre;



                _dbcontext.Cargos.Update(Cargos);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " Se Actualizo Correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdCargo:int}")]
        public IActionResult Eliminar(int IdCargo)
        {

            Cargo Cargos = _dbcontext.Cargos.Find(IdCargo);

            if (Cargos == null)
            {
                return BadRequest(" no encontrado");

            }

            try
            {

                _dbcontext.Cargos.Remove(Cargos);
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