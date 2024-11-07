using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public MarcaController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var Marca = _dbcontext.Marcas.Select(r => new
                {
                    r.IdMarca,
                    r.Nombre,
                    r.IdModeloMarca,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = Marca });
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
                var query = _dbcontext.Marcas.AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(m => m.Nombre.Contains(nombre));
                }

                var marcas = query.Select(r => new
                {
                    r.IdMarca,
                    r.Nombre,
                    r.IdModeloMarca
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = marcas });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("Obtener/{IdMarca:int}")]
        public IActionResult Obtener(int IdMarca)
        {
            Marca Marcas = _dbcontext.Marcas.Find(IdMarca);

            if (Marcas == null)
            {
                return BadRequest(" lo siento El Rol no existe");

            }

            try
            {

                var Marca = _dbcontext.Marcas.Select(r => new
                {
                    r.IdMarca,
                    r.Nombre,
                    r.IdModeloMarca,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = Marcas });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Marca objeto)
        {
            try
            {
                // Verificar si ya existe una marca con el mismo nombre
                var existeMarca = _dbcontext.Marcas
                    .Any(m => m.Nombre.ToLower() == objeto.Nombre.ToLower());

                if (existeMarca)
                {
                    return StatusCode(409, new { mensaje = "Ya existe una marca con este nombre." }); // 409 Conflict
                }

                var creacionMarca = new Marca { Nombre = objeto.Nombre, IdModeloMarca = objeto.IdModeloMarca };

                _dbcontext.Marcas.Add(creacionMarca);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionMarca, mensaje = "Se ha creado y guardado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Marca objeto)
        {
            // Buscar la marca a editar
            Marca marcaExistente = _dbcontext.Marcas.Find(objeto.IdMarca);

            if (marcaExistente == null)
            {
                return BadRequest("No ha sido encontrada.");
            }

            // Verificar si ya existe una marca con el mismo nombre (excluyendo la actual)
            var existeMarca = _dbcontext.Marcas
                .Any(m => m.Nombre.ToLower() == objeto.Nombre.ToLower() && m.IdMarca != objeto.IdMarca);

            if (existeMarca)
            {
                return StatusCode(409, new { mensaje = "Ya existe una marca con este nombre." }); // 409 Conflict
            }

            try
            {
                // Actualizar los campos de la marca
                marcaExistente.Nombre = objeto.Nombre;
                marcaExistente.IdModeloMarca = objeto.IdModeloMarca;

                _dbcontext.Marcas.Update(marcaExistente);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }





        [HttpDelete]
        [Route("Eliminar/{IdMarca:int}")]
        public IActionResult Eliminar(int IdMarca)
        {
            Marca marca = _dbcontext.Marcas.Find(IdMarca);

            if (marca == null)
            {
                return BadRequest(new { mensaje = "Marca no encontrada." });
            }

            // Verificar si la marca está asociada a productos
            bool isAssociated = _dbcontext.Productos.Any(p => p.IdMarca == IdMarca); // Cambia según tu modelo

            if (isAssociated)
            {
                return BadRequest(new { mensaje = "No se puede eliminar la marca porque está asociada a uno o más productos." });
            }

            try
            {
                _dbcontext.Marcas.Remove(marca);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Se eliminó exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar la marca." });
            }
        }


    }
}