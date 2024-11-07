using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("PermitirTodo")]
    [ApiController]
    public class TipoEntradaController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public TipoEntradaController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var TipoEntrada = _dbcontext.TipoEntrada.Select(r => new

                {
                    r.IdTipoEntrada,
                    r.Nombre,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = TipoEntrada });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdTipoEntrada:int}")]
        public IActionResult Obtener(int IdTipoEntrada)
        {
            TipoEntrada TipoEntrada = _dbcontext.TipoEntrada.Find(IdTipoEntrada);

            if (TipoEntrada == null)
            {
                return BadRequest(" lo siento El tipo de movimiento  no existe");

            }

            try
            {

                var TipoEntradaCreacion = _dbcontext.TipoEntrada.Select(r => new
                {
                    r.IdTipoEntrada,
                    r.Nombre,


                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = TipoEntrada });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] TipoEntrada objeto)
        {
            try
            {
                var creacionTipoEntrada = new TipoEntrada { Nombre = objeto.Nombre, };


                _dbcontext.TipoEntrada.Add(creacionTipoEntrada);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionTipoEntrada, mensaje = "Su tipo de movimiento  se ha creado y Guardado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] TipoEntrada objeto)
        {
            TipoEntrada TipoEntrada = _dbcontext.TipoEntrada.Find(objeto.IdTipoEntrada);

            if (TipoEntrada == null)
            {
                return BadRequest(" lo siento su tipo movimiento no ha sido encontrado ");
            }

            try
            {
                TipoEntrada.Nombre = objeto.Nombre;



                _dbcontext.TipoEntrada.Update(TipoEntrada);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su peticion Se Actualizo Correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdTipoEntrada:int}")]
        public IActionResult Eliminar(int IdTipoEntrada)
        {

            TipoEntrada TipoEntrada = _dbcontext.TipoEntrada.Find(IdTipoEntrada);

            if (TipoEntrada == null)
            {
                return BadRequest("su tipo de entrada  no  ha sido encontrado");

            }

            try
            {

                _dbcontext.TipoEntrada.Remove(TipoEntrada);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su tipo Entrada Se Elimino Exitosamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

    }
}

