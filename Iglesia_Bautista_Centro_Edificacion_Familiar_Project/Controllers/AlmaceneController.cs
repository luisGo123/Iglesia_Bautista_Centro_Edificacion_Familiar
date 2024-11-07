using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO;
using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlmaceneController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public AlmaceneController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var almacenes = _dbcontext.Almacenes
                    .Select(r => new AlmaceneDTO
                    {
                        IdAlmacenes = r.IdAlmacenes,
                        Nombre = r.Nombre,
                        Ubicacion = r.Ubicacion,
                        Cedula = r.IdUsuarioNavigation != null  ? r.IdUsuarioNavigation.Cedula: null,
                        NombreUsuario = r.IdUsuarioNavigation != null ? r.IdUsuarioNavigation.NombreCompleto : null
                    }).ToList();


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "La Petición realizada fue exitosamente", response = almacenes });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("Buscar")]
        public IActionResult Buscar( string? Ubicacion, string? Nombre)
        {
            try
            {
                var query = _dbcontext.Almacenes.AsQueryable();

                if (!string.IsNullOrEmpty(Ubicacion))
                {
                    query = query.Where(u => u.Ubicacion.Contains(Ubicacion));
                }

                if (!string.IsNullOrEmpty(Nombre))
                {
                    query = query.Where(u => u.Nombre.Contains(Nombre));
                }

             



                var alamaces = query.Select(r => new
                {
                    r.IdAlmacenes,
                    r.Nombre,
                    r.Ubicacion,
                    r.IdUsuario,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = alamaces });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdAlmacenes:int}")]
        public IActionResult Obtener(int IdAlmacenes)
        {
            Almacene almacenes = _dbcontext.Almacenes.Find(IdAlmacenes);

            if (almacenes == null)
            {
                return BadRequest(" lo siento El almacen  no existe");

            }

            try
            {

                var almacenesCreacion = _dbcontext.Almacenes.Select(r => new
                {
                    r.IdAlmacenes,
                    r.Nombre,
                    r.Ubicacion,
                    r.IdUsuario,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = almacenes });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Almacene objeto)
        {
            try
            {
               
                var existeAlmacen = _dbcontext.Almacenes
                    .Any(u => u.Nombre == objeto.Nombre);

                if (existeAlmacen)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un Almacen con este Nombre." }); // 409 Conflict
                }

                var creacionAlmacen = new Almacene
                {
                    Nombre = objeto.Nombre,
                    Ubicacion = objeto.Ubicacion,
                    IdUsuario = objeto.IdUsuario,
                 
                };

                _dbcontext.Almacenes.Add(creacionAlmacen);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionAlmacen, mensaje = "Su Almacen se ha creado y guardado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

   


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Almacene objeto)
        {
            Almacene almacene = _dbcontext.Almacenes.Find(objeto.IdAlmacenes);

            if (almacene == null)
            {
                return BadRequest("Lo siento, su almacene no ha sido encontrado.");
            }


            var existeAlmacen = _dbcontext.Almacenes
                .Any(u => u.Nombre == objeto.Nombre && u.IdAlmacenes != objeto.IdAlmacenes);

            if (existeAlmacen)
            {
                return StatusCode(409, new { mensaje = "Ya existe otro Almacen con este Nombre." }); // 409 Conflict
            }

            try
            {
                almacene.Nombre = objeto.Nombre;
                almacene.Ubicacion = objeto.Ubicacion;
                almacene.IdUsuario = objeto.IdUsuario;
        

                _dbcontext.Almacenes.Update(almacene);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su almacene se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{IdAlmacenes:int}")]
        public IActionResult Eliminar(int IdAlmacenes)
        {
            Almacene almacene = _dbcontext.Almacenes.Find(IdAlmacenes);

            if (almacene == null)
            {
                return BadRequest("almacene no encontrado.");
            }

          
            var AlmacenEnInventario = _dbcontext.Inventarios.Any(a => a.IdAlmacenes == IdAlmacenes);

            if (AlmacenEnInventario)
            {
                return StatusCode(409, new { mensaje = "No se puede eliminar el Almacen porque está asociado a un Inventario." }); // 409 Conflict
            }

            try
            {
                _dbcontext.Almacenes.Remove(almacene);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su Almacen se eliminó exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


    }
}
