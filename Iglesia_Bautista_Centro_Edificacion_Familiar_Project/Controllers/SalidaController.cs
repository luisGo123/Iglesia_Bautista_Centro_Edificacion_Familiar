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
    public class SalidaController : ControllerBase
    {


        private readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public SalidaController(dbIglesia_Bautista_Centro_FamiliarContext context)
        {
            _dbcontext = context;
        }

        // Método para crear una nueva salida
        [HttpPost("Guardar")]
        public async Task<IActionResult> CrearSalida([FromBody] Salida nuevaSalida)
        {
            if (nuevaSalida == null || nuevaSalida.Cantidad == null || nuevaSalida.IdProducto == null)
            {
                return BadRequest("Datos incompletos.");
            }

            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                var producto = await _dbcontext.Productos.FindAsync(nuevaSalida.IdProducto);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado.");
                }

                if ((producto.Existencia ?? 0) < nuevaSalida.Cantidad)
                {
                    return BadRequest("No hay suficiente inventario para realizar la salida.");
                }

                _dbcontext.Salida.Add(nuevaSalida);
                producto.Existencia -= nuevaSalida.Cantidad;
                _dbcontext.Productos.Update(producto);
                await _dbcontext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(nuevaSalida);
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Ocurrió un error al crear la salida.");
            }
        }

        // Método para listar todas las salidas con la información del producto y sus detalles adicionales
        [HttpGet("Listar")]
        public async Task<IActionResult> ListarSalidas()
        {
            try
            {
                var salidas = await _dbcontext.Salida
                    .Include(s => s.IdProductoNavigation)
                        .ThenInclude(p => p.IdCategoriaNavigation)
                    .Include(s => s.IdProductoNavigation)
                        .ThenInclude(p => p.IdTipoProductoNavigation)
                    .Include(s => s.IdProductoNavigation)
                        .ThenInclude(p => p.IdMarcaNavigation)
                    .Select(s => new SalidaDto
                    {
                        IdSalida = s.IdSalida,
                        IdProducto = s.IdProducto,
                        Cantidad = s.Cantidad,
                        NombreProducto = s.IdProductoNavigation.NombreProducto,
                        Color = s.IdProductoNavigation.Color,
                        Existencia = s.IdProductoNavigation.Existencia,
                        Categoria = s.IdProductoNavigation.IdCategoriaNavigation.Nombre, // Asumiendo que 'NombreCategoria' es el campo en Categorium
                        TipoProducto = s.IdProductoNavigation.IdTipoProductoNavigation.Nombre, // Asumiendo que 'NombreTipoProducto' es el campo en TipoProducto
                        Marca = s.IdProductoNavigation.IdMarcaNavigation.Nombre // Asumiendo que 'NombreMarca' es el campo en Marca
                    })
                    .ToListAsync();

                return Ok(salidas);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error al obtener la lista de salidas.");
            }
        }


        [HttpDelete("Eliminar/{idSalida}")]
        public async Task<IActionResult> EliminarSalida(int idSalida)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                // Buscar la salida por id
                var salida = await _dbcontext.Salida
                    .Include(s => s.IdProductoNavigation)
                    .FirstOrDefaultAsync(s => s.IdSalida == idSalida);

                if (salida == null)
                {
                    return NotFound("La salida no fue encontrada.");
                }

                // Obtener el producto y restablecer la cantidad eliminada
                var producto = salida.IdProductoNavigation;
                if (producto != null)
                {
                    producto.Existencia += salida.Cantidad;
                    _dbcontext.Productos.Update(producto);
                }

                // Eliminar la salida
                _dbcontext.Salida.Remove(salida);
                await _dbcontext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok($"Salida con ID {idSalida} eliminada correctamente y la existencia del producto ha sido actualizada.");
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Ocurrió un error al eliminar la salida.");
            }
        }

    }
}

