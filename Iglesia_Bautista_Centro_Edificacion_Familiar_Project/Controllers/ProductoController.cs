using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO;
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
    public class ProductoController : ControllerBase
    {
        private readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public ProductoController(dbIglesia_Bautista_Centro_FamiliarContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var productos = _dbcontext.Productos
                    .Include(p => p.IdCategoriaNavigation) 
                    .Include(p => p.IdTipoProductoNavigation) 
                    .Include(p => p.IdMarcaNavigation) 
                    .Select(r => new ProductoDTO 
                    {
                        IdProducto = r.IdProducto,
                        NombreProducto = r.NombreProducto,
                        Color = r.Color,
                        IdCategoria = r.IdCategoria ?? 0, 
                        CategoriaNombre = r.IdCategoriaNavigation.Nombre, 
                        IdTipoProducto = r.IdTipoProducto ?? 0, 
                        TipoProductoNombre = r.IdTipoProductoNavigation.Nombre, 
                        IdMarca = r.IdMarca ?? 0, 
                        MarcaNombre = r.IdMarcaNavigation.Nombre,
                        Existencia = r.Existencia ?? 0, 
                    })
                    .ToList();

                
                var totalProductos = _dbcontext.Productos.Count();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    mensaje = "La petición realizada fue exitosa",
                    response = productos,
                    totalProductos = totalProductos 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("BuscarPorNombre/{nombre}")]
        public IActionResult BuscarPorNombre(string nombre)
        {
            try
            {
                var productos = _dbcontext.Productos
                    .Where(r => r.NombreProducto.Contains(nombre)) // Busca productos que contengan el nombre
                    .Select(r => new
                    {
                        r.IdProducto,
                        r.NombreProducto,
                        r.Color,
                        r.IdCategoria,
                        r.IdTipoProducto,
                        r.IdMarca,
                           r.Existencia,
                    })
                    .ToList();

                if (productos.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No se encontraron productos que coincidan." });
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "La búsqueda fue exitosa", response = productos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdProducto:int}")]
        public IActionResult Obtener(int IdProducto)
        {
            Producto Productos = _dbcontext.Productos.Find(IdProducto);

            if (Productos == null)
            {
                return BadRequest(" lo siento el producto no existe");

            }

            try
            {

                var Producto = _dbcontext.Productos.Select(r => new
                {
                    r.IdProducto,
                    r.NombreProducto,
                    r.Color,
                    r.IdCategoria,
                    r.IdTipoProducto,
                    r.IdMarca,
                    r.Existencia,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = Productos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }




        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                var productoExistente = _dbcontext.Productos
                    .FirstOrDefault(p => p.NombreProducto.ToLower() == objeto.NombreProducto.ToLower());

                if (productoExistente != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Ya existe un producto con ese nombre." });
                }

                var creacionProducto = new Producto
                {
                    NombreProducto = objeto.NombreProducto,
                    Color = objeto.Color,
                    IdCategoria = objeto.IdCategoria,
                    IdTipoProducto = objeto.IdTipoProducto,
                    IdMarca = objeto.IdMarca,
                    Existencia = objeto.Existencia,
                };

                _dbcontext.Productos.Add(creacionProducto);
                _dbcontext.SaveChanges();

                // Contar la cantidad total de productos en la base de datos
                var totalProductos = _dbcontext.Productos.Count();

                return StatusCode(StatusCodes.Status200OK, new { savedProduct = creacionProducto, totalProductos = totalProductos, mensaje = "El Producto se ha Guardado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            if (objeto == null || objeto.IdProducto <= 0)
            {
                return BadRequest("Datos inválidos.");
            }

            var productoExistente = _dbcontext.Productos.Find(objeto.IdProducto);
            if (productoExistente == null)
            {
                return NotFound(new { mensaje = "Lo siento, su producto no ha sido encontrado." });
            }

            // Verificar si hay otro producto con el mismo nombre (exceptuando el actual)
            var productoConMismoNombre = _dbcontext.Productos
                .FirstOrDefault(p => p.NombreProducto.ToLower() == objeto.NombreProducto.ToLower() && p.IdProducto != objeto.IdProducto);

            if (productoConMismoNombre != null)
            {
                return BadRequest(new { mensaje = "Ya existe un producto con ese nombre." });
            }

            try
            {
                // Actualizar propiedades
                productoExistente.NombreProducto = objeto.NombreProducto;
                productoExistente.Color = objeto.Color;
                productoExistente.IdCategoria = objeto.IdCategoria;
                productoExistente.IdTipoProducto = objeto.IdTipoProducto;
                productoExistente.IdMarca = objeto.IdMarca;
                productoExistente.Existencia = objeto.Existencia;

                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "Se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{IdProducto:int}")]
        public IActionResult Eliminar(int IdProducto)
        {

            Producto Productos = _dbcontext.Productos.Find(IdProducto);

            if (Productos == null)
            {
                return BadRequest(" no encontrado");

            }

            try
            {

                _dbcontext.Productos.Remove(Productos);
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