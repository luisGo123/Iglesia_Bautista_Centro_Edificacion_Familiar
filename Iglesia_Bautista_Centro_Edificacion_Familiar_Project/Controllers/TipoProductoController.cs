using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("PermitirTodo")]
    [ApiController]
    public class TipoProductoController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public TipoProductoController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var Tipo_Productos = _dbcontext.TipoProductos.Select(r => new

                {
                    r.IdTipoProducto,
                    r.Nombre,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = Tipo_Productos });
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
                var query = _dbcontext.TipoProductos.AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(tp => tp.Nombre.Contains(nombre));
                }

                var tipoProductos = query.Select(r => new
                {
                    r.IdTipoProducto,
                    r.Nombre
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = tipoProductos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }



        [HttpGet]
        [Route("Obtener/{IdTipoProducto:int}")]
        public IActionResult Obtener(int IdTipoProducto)
        {
            TipoProducto tipoProductos = _dbcontext.TipoProductos.Find(IdTipoProducto);

            if (tipoProductos == null)
            {
                return BadRequest(" lo siento El tipo de producto  no existe");

            }

            try
            {

                var TipoProductoCreacion = _dbcontext.TipoProductos.Select(r => new
                {
                    r.IdTipoProducto,
                    r.Nombre,


                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = tipoProductos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] TipoProducto objeto)
        {
            try
            {
                // Verificar si ya existe un tipo de producto con el mismo nombre
                var existeTipoProducto = _dbcontext.TipoProductos
                    .Any(tp => tp.Nombre.ToLower() == objeto.Nombre.ToLower());

                if (existeTipoProducto)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un tipo de producto con este nombre." }); // 409 Conflict
                }

                var creacionTipoProducto = new TipoProducto { Nombre = objeto.Nombre };

                _dbcontext.TipoProductos.Add(creacionTipoProducto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionTipoProducto, mensaje = "Su tipo de producto se ha creado y guardado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] TipoProducto objeto)
        {
            // Buscar el tipo de producto existente
            TipoProducto tipoProducto = _dbcontext.TipoProductos.Find(objeto.IdTipoProducto);

            if (tipoProducto == null)
            {
                return BadRequest("Lo siento, su tipo de producto no ha sido encontrado.");
            }

            try
            {
                // Verificar si ya existe otro tipo de producto con el mismo nombre
                var existeTipoProducto = _dbcontext.TipoProductos
                    .Any(tp => tp.Nombre.ToLower() == objeto.Nombre.ToLower() && tp.IdTipoProducto != objeto.IdTipoProducto);

                if (existeTipoProducto)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un tipo de producto con este nombre." }); // 409 Conflict
                }

                // Actualizar el nombre del tipo de producto
                tipoProducto.Nombre = objeto.Nombre;

                _dbcontext.TipoProductos.Update(tipoProducto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su tipo de producto se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }





        [HttpDelete]
        [Route("Eliminar/{IdTipoProducto:int}")]
        public IActionResult Eliminar(int IdTipoProducto)
        {
            TipoProducto tipoProducto = _dbcontext.TipoProductos.Find(IdTipoProducto);

            if (tipoProducto == null)
            {
                return BadRequest(new { mensaje = "Tipo de producto no encontrado." });
            }

            // Verificar si el tipo de producto está asociado a algún producto
            bool isAssociated = _dbcontext.Productos.Any(p => p.IdTipoProducto == IdTipoProducto); // Cambia según tu modelo

            if (isAssociated)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el tipo de producto porque está asociado a uno o más productos." });
            }

            try
            {
                _dbcontext.TipoProductos.Remove(tipoProducto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El tipo de producto se eliminó exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar el tipo de producto." });
            }
        }


    }
}