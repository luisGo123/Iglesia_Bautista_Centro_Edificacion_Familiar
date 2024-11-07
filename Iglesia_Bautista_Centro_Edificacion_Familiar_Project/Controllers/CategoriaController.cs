using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

 
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public CategoriaController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var Categoria = _dbcontext.Categoria.Select(r => new
                {
                    r.IdCategoria,
                    r.Nombre,


                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = Categoria });
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
                var query = _dbcontext.Categoria.AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(c => c.Nombre.Contains(nombre));
                }

                var categorias = query.Select(r => new
                {
                    r.IdCategoria,
                    r.Nombre
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = categorias });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }




        [HttpGet]
        [Route("Obtener/{idCategoria:int}")]
        public IActionResult Obtener(int idCategoria)
        {
            // Busca la categoría específica por ID
            var categoria = _dbcontext.Categoria
                .Where(r => r.IdCategoria == idCategoria)
                .Select(r => new
                {
                    r.IdCategoria,
                    r.Nombre,
                })
                .FirstOrDefault();

            if (categoria == null)
            {
                return BadRequest("Lo siento, no existe");
            }

            try
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }



        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Categoria objeto)
        {
            try
            {
                // Verificar si ya existe una categoría con el mismo nombre
                var existeCategoria = _dbcontext.Categoria.Any(c => c.Nombre == objeto.Nombre);

                if (existeCategoria)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new { mensaje = "La categoría ya existe." });
                }

                var creacionCategoria = new Categoria { Nombre = objeto.Nombre };

                _dbcontext.Categoria.Add(creacionCategoria);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionCategoria, mensaje = "Se ha creado y guardado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Categoria objeto)
        {
            if (objeto == null || objeto.IdCategoria <= 0)
            {
                return BadRequest("Datos inválidos.");
            }

            try
            {
                var categoriaExistente = _dbcontext.Categoria.Find(objeto.IdCategoria);

                if (categoriaExistente == null)
                {
                    return NotFound(new { mensaje = "La categoría no ha sido encontrada." });
                }

                // Verificar si ya existe una categoría con el nuevo nombre
                var existeCategoria = _dbcontext.Categoria.Any(c => c.Nombre == objeto.Nombre && c.IdCategoria != objeto.IdCategoria);

                if (existeCategoria)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new { mensaje = "Ya existe una categoría con ese nombre." });
                }

                categoriaExistente.Nombre = objeto.Nombre;

                _dbcontext.Categoria.Update(categoriaExistente);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdCategoria:int}")]
        public IActionResult Eliminar(int IdCategoria)
        {
            Categoria categoria = _dbcontext.Categoria.Find(IdCategoria);

            if (categoria == null)
            {
                return BadRequest(new { mensaje = "Categoría no encontrada." });
            }

            // Verificar si la categoría está asociada a productos
            bool isAssociated = _dbcontext.Productos.Any(p => p.IdCategoria == IdCategoria); // Cambia según tu modelo

            if (isAssociated)
            {
                return BadRequest(new { mensaje = "No se puede eliminar la categoría porque está asociada a uno o más productos." });
            }

            try
            {
                _dbcontext.Categoria.Remove(categoria);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Se eliminó exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar la categoría." });
            }
        }


    }
}