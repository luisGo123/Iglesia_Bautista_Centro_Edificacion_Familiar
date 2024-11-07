using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloMarcaController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public ModeloMarcaController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var ModeloMarca = _dbcontext.ModeloMarcas.Select(r => new
                {
                    r.IdModeloMarca,
                    r.Nombre,


                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = ModeloMarca });
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
                var query = _dbcontext.ModeloMarcas.AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(m => m.Nombre.Contains(nombre));
                }

                var modeloMarcas = query.Select(r => new
                {
                    r.IdModeloMarca,
                    r.Nombre
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = modeloMarcas });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("Obtener/{IdModeloMarca:int}")]
        public IActionResult Obtener(int IdModeloMarca)
        {
            // Busca el modelo específico por IdModeloMarca
            var ModeloMarca = _dbcontext.ModeloMarcas
                                         .Where(r => r.IdModeloMarca == IdModeloMarca)
                                         .Select(r => new
                                         {
                                             r.IdModeloMarca,
                                             r.Nombre,
                                         })
                                         .FirstOrDefault(); // Obtén solo el primer resultado

            if (ModeloMarca == null)
            {
                return BadRequest("Lo siento, no existe");
            }

            try
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = ModeloMarca });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] ModeloMarca objeto)
        {
            try
            {
                // Verificar si ya existe un modelo con el mismo nombre
                var existeModelo = _dbcontext.ModeloMarcas
                    .Any(m => m.Nombre.ToLower() == objeto.Nombre.ToLower());

                if (existeModelo)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un modelo con este nombre." }); // 409 Conflict
                }

                var creacionModelo = new ModeloMarca { Nombre = objeto.Nombre };

                _dbcontext.ModeloMarcas.Add(creacionModelo);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionModelo, mensaje = "Se ha creado y guardado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }




        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] ModeloMarca objeto)
        {
            var modeloExistente = _dbcontext.ModeloMarcas.Find(objeto.IdModeloMarca);

            if (modeloExistente == null)
            {
                return BadRequest("Lo siento, no ha sido encontrado.");
            }

            try
            {
                // Verificar si el nuevo nombre ya existe, excluyendo el modelo actual
                var existeModelo = _dbcontext.ModeloMarcas
                    .Any(m => m.Nombre.ToLower() == objeto.Nombre.ToLower() && m.IdModeloMarca != objeto.IdModeloMarca);

                if (existeModelo)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un modelo con este nombre." }); // 409 Conflict
                }

                // Actualizar el nombre
                modeloExistente.Nombre = objeto.Nombre;

                _dbcontext.ModeloMarcas.Update(modeloExistente);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }



        [HttpDelete]
        [Route("Eliminar/{IdModeloMarca:int}")]
        public IActionResult Eliminar(int IdModeloMarca)
        {
            ModeloMarca modeloMarca = _dbcontext.ModeloMarcas.Find(IdModeloMarca);

            if (modeloMarca == null)
            {
                return BadRequest(new { mensaje = "Modelo de marca no encontrado." });
            }

            // Verificar si el modelo de marca está asociado a marcas
            bool isAssociated = _dbcontext.Marcas.Any(m => m.IdModeloMarca == IdModeloMarca); // Cambia según tu modelo

            if (isAssociated)
            {
                return BadRequest(new { mensaje = "No se puede eliminar el modelo de marca porque está asociado a una o más marcas." });
            }

            try
            {
                _dbcontext.ModeloMarcas.Remove(modeloMarca);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Se eliminó exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar el modelo de marca." });
            }
        }


    }
}
    
